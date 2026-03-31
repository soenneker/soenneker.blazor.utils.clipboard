export class ClipboardInterop {
    hasClipboard() {
        return typeof navigator !== "undefined" && navigator.clipboard != null;
    }

    readText() {
        return navigator.clipboard.readText();
    }

    writeText(text) {
        return navigator.clipboard.writeText(text ?? "");
    }

    async getReadPermissionState() {
        try {
            if (!navigator.permissions || !navigator.permissions.query)
                return "unsupported";
            const result = await navigator.permissions.query({ name: "clipboard-read" });
            return result.state;
        } catch {
            return "unsupported";
        }
    }

    async getWritePermissionState() {
        try {
            if (!navigator.permissions || !navigator.permissions.query)
                return "unsupported";
            const result = await navigator.permissions.query({ name: "clipboard-write" });
            return result.state;
        } catch {
            return "unsupported";
        }
    }

    async read() {
        const items = await navigator.clipboard.read();
        const result = [];
        for (const item of items) {
            const types = {};
            for (const type of item.types) {
                const blob = await item.getType(type);
                const isText = type.startsWith("text/") || type === "application/json";
                if (isText) {
                    types[type] = await blob.text();
                } else {
                    types[type] = await this._blobToDataUrl(blob);
                }
            }
            result.push({ Types: types });
        }
        return result;
    }

    _blobToDataUrl(blob) {
        return new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.onload = () => resolve(reader.result);
            reader.onerror = () => reject(reader.error);
            reader.readAsDataURL(blob);
        });
    }

    async write(items) {
        const clipboardItems = [];
        for (const item of items) {
            const types = item.Types || item;
            const entries = {};
            for (const [type, value] of Object.entries(types)) {
                if (typeof value !== "string") continue;
                if (value.startsWith("data:") && value.includes(";base64,")) {
                    const blob = await this._dataUrlToBlob(value);
                    entries[type] = blob;
                } else {
                    entries[type] = new Blob([value], { type });
                }
            }
            if (Object.keys(entries).length > 0) {
                clipboardItems.push(new ClipboardItem(entries));
            }
        }
        if (clipboardItems.length > 0) {
            await navigator.clipboard.write(clipboardItems);
        }
    }

    _dataUrlToBlob(dataUrl) {
        return fetch(dataUrl).then((r) => r.blob());
    }
}

window.ClipboardInterop = new ClipboardInterop();
