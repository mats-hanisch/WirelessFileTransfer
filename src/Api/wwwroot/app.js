async function uploadFiles() {
    const uploadButton = document.getElementById("uploadButton");
    const input = document.getElementById("fileInput");
    const files = input.files;
    
    if (!files.length) {
        alert("Bitte wähle mindestens eine Datei aus.")
        return;
    }

    uploadButton.disabled = true;

    const progressDiv = document.getElementById("progressDiv");
    progressDiv.innerHTML = "";

    for (const file of files) {

        const responseDiv = document.createElement("div");
        responseDiv.innerText = `"${file.name}" wird hochgeladen ...`;
        progressDiv.appendChild(responseDiv);

        try {
            const response = await fetch("/upload-file", {
                method: "POST",
                headers: {
                    "Filename": file.name
                },
                body: file
            });

            if (!response.ok) {
                console.log(response.status)
                throw new Error();
            }

            responseDiv.innerText = `Hochgeladen: "${file.name}"`;
        }
        catch (_) {
            console.log(_);
            responseDiv.innerText = `Fehler bei "${file.name}"`;
            responseDiv.classList.add("error");
        }
    }

    uploadButton.disabled = false;
}