Title: About This Blog
Date: 2014-12-01 
==
I'm starting a new blog, my third one, here on Chpokk, because this is my current project, and this is where my focus and my energy is at the moment.
--
I means me, Artёm Smirnov, with this funny "ё" character in my name, which I hope renders correctly for even you ASCII guys who live far away across the Big Blue Sea.

I'll be writing here about various features that Chpokk has, and how I implemented them. I *might* be writing something about marketing, mainly a link to somebody else's wisdom (since I don't have much of my own yet), but it won't be much.

Let's get started.

## Creating a text-based blog

While I got tired of relational databases, I'm also not courageous enough (yet) to jump onto noSQL development. So I've been trying this new minimalistic idea -- don't use a database until you need it. For a year, I managed to run Chpokk **without any database at all**. Finally I started using it for stuff like keeping user and usage data. 

So, storing my blog posts in the file system seemed like a good idea. The only thing I had to think about was how to store the associated metadata (title, pubdate, tags). At one moment I almost decided to create a separate XML file for it, but changed my mind. The final version looks like this:
```markdown
Title: <..>
Date: <..>
<I can add more fields when I need them>
==	
<Here's the text I want to appear in the list of posts, as well as in the beginning of the actual post>
--	
<The rest of the post>
```

For parsing Markdown I chose Tanka.Markdown. This is not the most popular parser, but I found it easily extensible (although haven't had need for that yet), being able to produce HTML documents (based on the Fubu's HTMLTags library, great for testing), and a great piece of code (some of the classes might be useful in other scenarios). It even parses codeblocks correctly, adding a codelang attribute!

The current implementation is pretty minimalistic. I list all my post titles and descriptions on one page with links to full posts. There's no tags, comments, or even sorting by date. There's no caching either, which is OK since the traffic is pretty low. Yes, I do loading and parsing on every request. My aim was to release the first post as fast as possible and tell Google about it.

So, I'd say it's been fun coding it! Now I can write my posts in a text editor, without building a backend, and store them in my local repository, then push with the rest of the changes to AppHarbor. It'll only take a few minutes to add caching, and a bit more to add tags, RSS, maybe other stuff that blogs have. But for now, I'm pretty happy with the result, and I'm moving to the more pressing stuff.

Until next time.. 

