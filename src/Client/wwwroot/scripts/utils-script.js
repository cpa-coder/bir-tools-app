function downloadFromUrl(fileName, url) {
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
}

function downloadZip(fileName, byte) {
    let blob = new Blob([byte], {type: "application/zip"});
    const anchorElement = document.createElement('a');
    anchorElement.href = window.URL.createObjectURL(blob);
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
}

function preferDarkMode() {
    return window.matchMedia("(prefers-color-scheme:dark)").matches;
}