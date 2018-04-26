# IDeliverable.Seo

A module for the Orchard CMS that helps site owners optimize their website for **search engines** and improve discoverability by enabling robust and granular control over SEO aspects. The module lets site owners control **page titles**, **meta keywords and descriptions**, **robots.txt** and **sitemap.xml**. Additionally, it allows **301/302 redirects** to be configured for changed URLs.

Search Engine Optimization (SEO) is a must. If your site is easily crawled and index by search engines such as Bing and Google, chances are your site looks better and ranks higher on their search results pages. Optimizing a site for search engines involves many things, such as providing fresh content, relevant URLS, keywords and descriptions, proper use of heading tags, providing a sitemap, and so on. In order to be able to do all that, webmasters need tools that let them control all of these aspects. That's where **IDeliverable.Seo** comes in - an Orchard module that lets site owners and content editors control every aspect related to SEO on a granular level.

## Features

### 301/302 Redirects

The module comes featured with an alternate alias management editor, making it a breeze to keep your old URLS while creating new ones for your content. Old URLS will redirect the user using a 301 or 302 HTTP status code, which is configurable by the editor.

### Easy to use

Both setting up as well as using the SEO tools could not be simpler. The module comes with a set of *recipes* that can be executed from the *Modules* section for quick setup using the most common configuration, such as attaching the `SeoPart` and optionally the `SeoSiteMapPart` to the `Page` content type and enabling the `Sitemap.xml` and `Robots.txt` features.

The various screens and content parts all provide easy-to-understand UI, making it a breeze to provide content and configuration relevant to search engines.

### Integrates organically

Control over the browser's Title bar, meta keywords and descriptions and meta robots are all configurable on a per content item basis by means of the **SeoPart** that you attach to your content types.

The module also comes with a **sitemap.xml route** that generates an XML structure automatically for you. However, the **SeoSitemapPart** allows for even more control over how each entry appears in the sitemap document by allowing control over aspects such as *change frequency* and *priority*.

### Extensible

Orchard is all about extensibility, and so are our Orchard modules. Developers can extend the functionality of the **IDeliverable.Seo** module with new *Sitemap providers*.

## Key Concepts


### Title

With *title*, we refer to the `<title>` tag in the `<head>` element of your webpage. Controlling this aspect is key when it comes to SEO. Where ordinarily this title would be generated based on the content title and the site name, **IDeliverable.Seo** provides you with full manual control over this aspect.

### Meta tags

Similar to the `<title>` tag, `<meta>` tags are another important aspect to SEO. The module supports any meta tag you need, and has specialized support for the the keywords, description and robots meta tags.

### Sitemap.xml

**IDeliverable.Seo** provides a root **sitemap.xml** URL and generates its contents for xml dynamically based on your site's content and navigation structure, and lets you control the *change frequency* and *priority* on a per-content item basis. It even has support for automatically generating multiple sitemaps.

### Robots.txt

The **IDeliverable.Seo** module makes it easy to provide this file. Simply go to the Robots section under the Seo admin menu item and provide the contents. The module will automatically serve the contents at the *www.yoursite.com/robots.txt* URL.

### Multiple Aliases and 301/302 Redirects

The **IDeliverable.Seo** modules comes with a feature called **Seo Routes**, which provides a `RoutesPart` that you can attach to your content types. Doing so will enable the editor to provide multiple aliases to a content item and specifying a behavior to those aliases. For example, a 301 or 302 redirect. This will make changing URLs painless because web crawlers requesting the old URL will simply receive a 301 or 302 redirect and follow the updated URL. Easy.

### Extensibility

Just as **IDeliverable.Seo** uses the extensibility of the Orchard CMS to integrate with Orchard in an organic and natural way, it also mirrors this philosophy and itself provides extensibility points, which you as a developer can utilize to extend the functionality of the module even further:

1. Developers can create new *sitemap providers* by implementing the `ISitemapHandler` interface or inheriting from `SitemapHandlerBase`. A sitemap handler is a component provides a collection of sitemap entries to be included in the resulting sitemap.xml document. Out of the box, the module provides four providers: `CustomEntrySitemapHandler`, `NavigationSitemapHandler`, `RoutableContentSitemapHandler` and `TagSitemapHandler`.

## Getting Started

### Installation

To install the module from Orchard Gallery:

1. In the Orchard admin UI, navigate to **Modules -> Gallery**.
1. Search for "IDeliverable.Seo".
1. Click **Install** to install the module.

To install the module using the ZIP file:

1. Download the [module ZIP file](https://github.com/IDeliverable/IDeliverable.Seo/archive/orchard-1.10.x.zip).
1. Unzip the contents into the `Modules` folder of your Orchard installation (this creates an `IDeliverable.Seo` subfolder).

To integrate the module into your development workflow, unzip the contents into the `Orchard.Web\Modules` folder of your local repository and add it to source control (if any).

### Basic configuration

1. Enable the feature **Seo**.
1. Attach the **Seo** content part to the **Page** content type.
1. Optionally, enable one or more of the optional features **Seo Sitemap**, **Seo Sitemap for Tags** and **Seo Robots**.

Alternatively, execute one of the following recipes: **Robots**, **Seo Part**, **Sitemap** or **Sitemap for Tags**. Each of these will enable the required features as well as provide some basic configuration, such as attaching the **Seo** content part to the **Page** content type in the case of the **Seo Part** recipe.

### System requirements and compatibility
		
**IDeliverable.Seo** is compatibility-tested and supported on **Orchard version 1.10.x**. The module might also work on older or newer versions of Orchard but this is not guaranteed.

We make a commitment that the current release of our modules should always work with the current minor release of Orchard (e.g. 1.10) and across all subsequent revision releases (e.g. 1.10.1, 1.10.2 and so on). We strive to always conduct compatibility testing (and release an updated module if necessary) within two weeks of every new Orchard release.

The module provides the following features with their respective dependencies:

- *Seo* (`IDeliverable.Seo`) depends on `Orchard.Resources`.
- *Seo Sitemap* (`IDeliverable.Seo.Sitemap`) depends on `IDeliverable.Seo` and `Orchard.Autoroute`.
- *Seo Sitemap for Tags* (`IDeliverable.Seo.Sitemap.Tags`) depends on `IDeliverable.Seo.Sitemap` and `Orchard.Tags`.
- *Seo Robots* (`IDeliverable.Seo.Robots`) depends on `IDeliverable.Seo`.
