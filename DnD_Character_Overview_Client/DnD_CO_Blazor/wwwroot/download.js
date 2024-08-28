window.BlazorDownloadFile = async (fileName, streamReference) => {
    const data = await streamReference.arrayBuffer();
    const blob = new Blob([data], { type: "application/octet-stream" });
    const url = URL.createObjectURL(blob);
    const anchor = document.createElement("a");
    anchor.href = url;
    anchor.download = fileName || "file.dat";
    anchor.click();
    URL.revokeObjectURL(url);
};