Title: About Chpokk
Date: 2014-12-22
==
Chpokk is an online C# and VB.Net editor and compiler with Intellisense and NuGet support
--
## Chpokk is an online C# and VB.Net code editor

You can edit your code using the famous Ace editor, which gives you code highlighting, brace completion, line numbering, and probably a zillion other useful things which I am still to discover.

Did you know you could maximize it? There's a tiny button just above the editing space for that, at the right side.

Common file operations include creating, renaming, moving, and deleting files. Creating is done via the "Add Item" menu, the rest is in the Solition Explorer at the left side. 
Rename a file by selecting it and typing in the new name, move it by dragging, delete by selecting it and pressing "Del".

## Chpokk is an online .Net compiler and syntax checker

Your code is checked for syntax errors just as you type. Not with every character (that would give too many false alarms), but on every Enter.

There's also Intellisense, so that you don't have to memorize all members of all libraries in the world. Some of you complained that Intellisense gives more problems than solutions, so now you can switch it off. It might be slower than you type, but it can be really useful sometimes, and it's getting better.

Finally, you can compile a single project (if you have one of its files open), or the whole solution. Click on the error output to get straight to the line which has the error.

## Chpokk is an online .Net IDE

You can create or upload your solutions and projects online. Currently creating 3 types of projects are supported: Console apps, libraries, and Web apps, but you can upload any type of project.

You can add references to .Net framework libraries, NuGet packages, uploaded assemblies, and, of course, other projects.

## Executing and unit testing your C# or VB.Net code online

If you have a console program, you can run it online using Chpokk, and see the output and the returned value in the output window.

Any kind of project can be tested by executing automated tests. Click "Run Tests" in the top menu, and all tests in ther solution will be run. The tests are run using the Gallio framework, so all major testing frameworks are supported.

Finally, a Web project can be published to, say, Azure or AppHarbor, by git-pushing it to the corresponding remote.

## Getting your code in and out of the system

If you have your code in publicly available source control, git clone or svn checkout it (other source control systems will be available on demand). You can also upload/download it all zipped.

## Concluding remarks

Despite Chpokk being the best .Net IDE ever, it still has a room for improvement. Project and item templates, TFS integration, command support, debugging, and many other features are waiting to be implemented. 

You can always use the orange thingy at the right side to send me your feature suggestions (or just share your excitement).

[Try it yourself!](/Main) You'll like it, I promise!