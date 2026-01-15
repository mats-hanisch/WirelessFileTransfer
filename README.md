# WirelessFileTransfer

WirelessFileTransfer is a **lightweight, self-contained Windows desktop application** that allows you to transfer files **wirelessly** to your PC using a browser — no cables, no pairing, no accounts.

After installation, the app starts a **local server** on your machine and displays a QR code containing your local IP address. Any device on the same network can scan the QR code, open the web interface, and **upload files directly to your PC**.

---

## Features

- **Wireless file transfer** over the local network
- **Self-contained** Windows desktop application
- **QR code** for quick access
- **Browser-based upload** interface
- **No installation** required on client devices
- **Runs entirely locally** (LAN only)
- **Server** automatically starts and stops with the application

---

## How It Works

1. [Install](#download--installation-recommended) and start WirelessFileTransfer
2. A window opens displaying:
   - A QR code containing the local IP address
   - The local IP address as text
   - A button to copy the address
3. Internally, a small local HTTP server starts on the PC
4. Scan the QR code or open the IP address in a browser on the same network
5. A simple web page is served containing a file upload form
6. Uploaded files are sent via an API endpoint and saved on the host machine
7. Closing the desktop application automatically shuts down the server

---

## Language Support

Currently, the desktop application UI and the web frontend are available **only in German**.

Additional languages may be added in the future.

---

## Download & Installation (Recommended)

If you only want to use the application, you do not need to clone the repository.

Download the prebuilt distribution from the GitHub Releases section.  
The release contains a fully packaged, ready-to-run Windows build.

---

## Technology Stack

### Backend Server
- .NET / C#
- ASP.NET Core (Minimal API)

### Desktop Application
- WPF (Windows Presentation Foundation)

### Web Frontend
- Vanilla HTML
- CSS
- JavaScript

### Tooling & Deployment
- PowerShell

---

## PowerShell Scripts

### publish.ps1 (build/publish.ps1)

Creates a self-contained, publishable distribution of the application.

- Builds the application
- Outputs the final artifacts to the `dist/bin/` directory

### deploy.ps1 (dist/deploy.ps1)

Deploys the generated installation (found in `dist/bin`).

- Finalizes installation
- Creates a desktop shortcut

---

## Development Setup

### Requirements

- Windows
- .NET SDK (matching the project version)
- PowerShell

### Build Steps

```powershell
   git clone https://github.com/mats-hanisch/WirelessFileTransfer.git
   cd WirelessFileTransfer
   ./build/publish.ps1
```

The finished build will be available in the `dist/bin` directory.

## Security Notes

- The local server is only accessible within the **local network**
- **No external services** or cloud infrastructure are used
- The server lifecycle is bound to the desktop application and **shuts down automatically on exit**

---

## License

This project is licensed under the **MIT License**.
