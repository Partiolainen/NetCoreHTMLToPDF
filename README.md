# .NET Core HTML to PDF converter

This is a .NET Core HTML to PDF converter. It helps converting HTML strings or URLs to PDF bytes. Please see the examples:

## Installation Instructions
Nuget package available (https://www.nuget.org/packages/NetCoreHTMLToPDF)
```
Install-Package NetCoreHTMLToPDF
```

## Convert HTML string to PDF bytes
```
var converter = new HtmlConverter();
var html = "<div><strong>Hello</strong> World!</div>";
var bytes = converter.FromHtmlString(html);
File.WriteAllBytes("html.pdf", bytes);
```
            
## Convert URL to PDF bytes
```
var converter = new HtmlConverter();
var bytes = converter.FromUrl("http://google.com");
File.WriteAllBytes("url.pdf", bytes);
```

## Optional parameters
1. pageSize - Set paper size to: A4, Letter, etc. (default A4)
2. pageOrientation - Set orientation to Landscape or Portrait (default Portrait)

## Roadmap
* Async interface

## For Ubuntu users
```
sudo wget https://github.com/wkhtmltopdf/packaging/releases/download/0.12.6-1/wkhtmltox_0.12.6-1.buster_amd64.deb
sudo dpkg -i wkhtmltox_0.12.6-1.buster_amd64.deb
sudo apt-get install -f
sudo ln -s /usr/local/bin/wkhtmltopdf /usr/bin
sudo ln -s /usr/local/bin/wkhtmltoimage /usr/bin
```
