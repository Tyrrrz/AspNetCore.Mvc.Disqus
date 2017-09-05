# Tyrrrz.AspNetCore.Mvc.Disqus

Tag helper used to render [Disqus](https://disqus.com) threads in ASP.net Core MVC views.

## Download

- Using nuget: `Install-Package Tyrrrz.AspNetCore.Mvc.Disqus`

## Usage

Make the tag helper available with the `addTagHelper` directive either in your view or `_ViewImports.cshtml`.

```
@addTagHelper *, Tyrrrz.AspNetCore.Mvc.Disqus
```

Use the tag helper to initialize the Disqus thread.
You will need to specify at least the `site` attribute, which is the shortname for your website as set in your Disqus dashboard.

```html
<disqus site="tyrrrzme" />
```

You can also specify the `page-url` and `page-id` attributes if you want.
If they are not set, the current request URL is used instead.
It's generally recommended to set `page-id` so that your threads can be bound to something less volatile than a URL.