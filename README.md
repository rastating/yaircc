# yaircc

yaircc (pronounced yerk) is a free, open-source IRC client for Windows that complies with the standards set by both RFC 1459 and RFC 2812, and also supports a number of defacto standards that have become a part of many IRC clients over the years, such as custom font colours, action messages and more.

To download the latest stable release visit https://www.yaircc.com/

## Author
- [rastating](http://blog.intninety.co.uk/) ([@iamrastating](https://twitter.com/iamrastating))

## Contributors
- [Asif Aleem](http://www.freebiesgallery.com/) ([@freebiegallery](https://twitter.com/freebiegallery)) - Splash Screen Design
- [Chris West](http://cwestblog.com/) - Author of a JavaScript prototype that is used to replace multiple literals.
- [Everaldo Coelho](http://www.everaldo.com/) ([@_Everaldo](https://twitter.com/_Everaldo)) - Graphic Design
- Mark James ([@markjames](https://twitter.com/markjames)) - Icon Design
- [Phyushin](https://github.com/phyushin) ([@phyushin](https://twitter.com/phyushin)) - Testing
- [ponix4k](https://github.com/ponix4k) ([@ponix4k](https://twitter.com/ponix4k)) - Testing
- [Prekesh](http://www.prekesh.com/) ([@prekesh](https://twitter.com/prekesh)) - Emoticon Design
- Yusuke Kamiyamane ([@ykamiyamane](http://twitter.com/ykamiyamane)) - Icon Design

## Contributing
If you want to contribute to the project ensure that all code written passes a StyleCop test using the settings found in the main project (https://github.com/intninety/yaircc/blob/master/yaircc/Settings.StyleCop). 

All comments must be in English (British English preferred for consistency, but any flavour will work as long as it's legible!).

Alternatively if you want to contribute in way of resources such as icons, [Open an Issue](https://github.com/rastating/yaircc/issues/new) describing how you want to contribute.

## Building the Project
To build the latest version of the project a number of dependencies will be required, which can be found here: http://dl.dropbox.com/u/50025441/yaircc-dependencies.zip

A reference must be added in the project to both CefSharp.dll and CefSharp.WinForms.dll, and the rest of the files must be present in the output directory in order to run.
