<h3 align="center">CatMarkdown</h3>

  <p align="center">
    A simple Markdown parser and renderer for .NET
    <br />
  </p>
</div>

<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
  </ol>
</details>

<!-- ABOUT THE PROJECT -->
## About The Project

[![Product Name Screen Shot][product-screenshot]](https://example.com)

CatMarkdown is a simple Markdown parser and renderer for .NET. It converts Markdown text into HTML, supporting various Markdown features such as headings, lists, blockquotes, code blocks, and inline formatting.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

### Built With

* .NET 9.0

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- GETTING STARTED -->
## Getting Started

To get a local copy up and running follow these simple steps.

### Prerequisites

* .NET 9.0 SDK

### Installation

1. Clone the repo
   ```sh
   git clone https://github.com/github_username/repo_name.git
   ```

2. Navigate to the project directory
    ```sh
    cd repo_name
    ```
3. Restore dependencies
    ```sh
    dotnet restore
    ```

## Usage
To use the Markdown parser and renderer, create instances of MarkdownParser and HTMLRenderer and call their methods:

    using CatMarkdown.src.Parsing;
    using CatMarkdown.src.Rendering;

    var parser = new MarkdownParser();
    var renderer = new HTMLRenderer();

    string markdown = "# Hello, World!";
    string parsedContent = parser.Parse(markdown);
    string html = renderer.Render(parsedContent);

    Console.WriteLine(html);
