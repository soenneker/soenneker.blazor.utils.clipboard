function getClipboard() {
    return typeof navigator !== "undefined" ? navigator.clipboard : undefined;
}

function blobToDataUrl(blob) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();

        reader.onload = () => resolve(reader.result);
        reader.onerror = () => reject(reader.error);

        reader.readAsDataURL(blob);
    });
}

function dataUrlToBlob(dataUrl) {
    return fetch(dataUrl).then((response) => response.blob());
}

export function hasClipboard() {
    return getClipboard() != null;
}

export function readText() {
    const clipboard = getClipboard();
    return clipboard.readText();
}

export function writeText(text) {
    const clipboard = getClipboard();
    return clipboard.writeText(text ?? "");
}

export async function getReadPermissionState() {
    try {
        if (!navigator.permissions || !navigator.permissions.query)
            return "unsupported";

        const result = await navigator.permissions.query({ name: "clipboard-read" });
        return result.state;
    } catch {
        return "unsupported";
    }
}

export async function getWritePermissionState() {
    try {
        if (!navigator.permissions || !navigator.permissions.query)
            return "unsupported";

        const result = await navigator.permissions.query({ name: "clipboard-write" });
        return result.state;
    } catch {
        return "unsupported";
    }
}

export async function read() {
    const clipboard = getClipboard();
    const items = await clipboard.read();
    const result = [];

    for (const item of items) {
        const types = {};

        for (const type of item.types) {
            const blob = await item.getType(type);
            const isText = type.startsWith("text/") || type === "application/json";

            types[type] = isText
                ? await blob.text()
                : await blobToDataUrl(blob);
        }

        result.push({ Types: types });
    }

    return result;
}

export async function write(items) {
    const clipboard = getClipboard();
    const clipboardItems = [];

    for (const item of items) {
        const types = item.Types || item;
        const entries = {};

        for (const [type, value] of Object.entries(types)) {
            if (typeof value !== "string")
                continue;

            entries[type] = value.startsWith("data:") && value.includes(";base64,")
                ? await dataUrlToBlob(value)
                : new Blob([value], { type });
        }

        if (Object.keys(entries).length > 0)
            clipboardItems.push(new ClipboardItem(entries));
    }

    if (clipboardItems.length > 0)
        await clipboard.write(clipboardItems);
}