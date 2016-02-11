Title: Hosting a .Net WinForms control in Internet Explorer and communicating with it via JavaScript
Date: 2016-01-19
==
Yes, I'm doing terrible, horrible things on my daytime job. One of these things even contains the word "Classic" in its name.

But I'm not complaining.

So, you probably don't care why, but I was tasked with creating a .Net control as a replacement for an existing ActiveX, which had been developed in ancient times where we had a guy in our team that really could do C++.

(BTW there were times when I also did *the pluses*, but after the first project (which involved a screensaver with a plastic Bill Gates leaving colorful footsteps all over your precious work) I promised myself to never come close to it).

Long story short, there are several articles that explain how to do it, but a common knowledge (see e.g. [this 4GuysFromRolla article](http://www.4guysfromrolla.com/articles/052604-1.aspx)) seems to be that you can't communicate with it effectively except for passing initial parameter values via HTML markup.
--
### This is wrong. You can.
I sure hope you'll never have a problem like that, so I'm only posting this because I spent many hours trying to figure this (that, and also to revitalize my blog, although this might be not the best topic).

```csharp
[assembly: ComVisible(true)]
```

Yes, this is all it takes. Actually, it takes even less, you only have to remove the `ComVisible(true)` line, but that wouldn't warrant a blog post, would it?