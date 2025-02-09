<a id="readme-top"></a>

<!-- PROJECT SHIELDS -->
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]

<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/github_username/repo_name">
    <img src="images/logo.png" alt="Logo" width="80" height="80">
  </a>

<h3 align="center">CatMarkdown</h3>

  <p align="center">
    A simple Markdown parser and renderer for .NET
    <br />
    <a href="https://github.com/github_username/repo_name"><strong>Explore the docs »</strong></a>
    <br />
    <br />
    <a href="https://github.com/github_username/repo_name">View Demo</a>
    &middot;
    <a href="https://github.com/github_username/repo_name/issues/new?labels=bug&template=bug-report---.md">Report Bug</a>
    &middot;
    <a href="https://github.com/github_username/repo_name/issues/new?labels=enhancement&template=feature-request---.md">Request Feature</a>
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
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
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
    ```C#
    using CatMarkdown.src.Parsing;
    using CatMarkdown.src.Rendering;

    var parser = new MarkdownParser();
    var renderer = new HTMLRenderer();

    string markdown = "# Hello, World!";
    string parsedContent = parser.Parse(markdown);
    string html = renderer.Render(parsedContent);

    Console.WriteLine(html);
    ```
