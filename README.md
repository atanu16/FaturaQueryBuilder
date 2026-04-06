# ⚡ Query Builder — Glassmorphism WPF App

A modern, stylish Windows desktop app built with **.NET 8 + WPF** that converts multi-line text into a single Outlook / Exchange subject search query.

![.NET 8](https://img.shields.io/badge/.NET-8.0-blueviolet)
![WPF](https://img.shields.io/badge/WPF-Windows-blue)

---

## 🎯 What It Does

Paste lines like:

```
Fatura ABC
Fatura XYZ
Fatura 123
```

Click **Generate Query** and get:

```
ABC " OR Subject : "Fatura XYZ " OR Subject : "Fatura 123 "
```

- Joins every line with `" OR Subject : "` separators
- Strips the leading `Fatura ` prefix from the first entry
- One-click **Copy to Clipboard**
- Animated **snackbar notifications** for every action

---

## 🖥️ Prerequisites

| Requirement | Version |
|-------------|---------|
| **.NET SDK** | 8.0 or later |
| **OS** | Windows 10 / 11 |

Download the SDK → https://dotnet.microsoft.com/download/dotnet/8.0

---

## 🚀 Build & Run

```bash
# 1. Open a terminal in the project folder
cd QueryBuilder

# 2. Restore & run
dotnet run

# — OR build a standalone EXE —
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
# Output: bin/Release/net8.0-windows/win-x64/publish/QueryBuilder.exe
```

---

## 🎨 Design Highlights

| Feature | Detail |
|---------|--------|
| **Glassmorphism panels** | Semi-transparent cards with soft borders and glow shadows |
| **Ambient light blobs** | Purple, cyan, and pink radial gradients behind content |
| **Custom title bar** | Draggable, with minimize / maximize / close buttons |
| **Animated snackbar** | Fades in + slides up; auto-dismisses after 3 s |
| **Gradient accent button** | Purple → Pink gradient with glow shadow |
| **Monospace I/O** | Cascadia Code / Consolas for the text areas |
| **Live stats bar** | Lines · Conditions · Characters counter |

---

## 📁 Project Structure

```
QueryBuilder/
├── QueryBuilder.csproj   # .NET 8 WPF project
├── App.xaml              # Global theme & resource dictionary
├── App.xaml.cs
├── MainWindow.xaml       # UI layout (glassmorphism)
├── MainWindow.xaml.cs    # Logic, clipboard, snackbar
└── README.md
```

---

## 🔧 Customisation

To change the search prefix (default `Fatura `), edit **MainWindow.xaml.cs**:

```csharp
const string prefix = "Fatura ";   // ← change this
```

To change the join separator, look for:

```csharp
string joined = string.Join(" \" OR Subject : \"", lines) + " \"";
```

---

Enjoy! 🚀
"# FaturaQueryBuilder" 
