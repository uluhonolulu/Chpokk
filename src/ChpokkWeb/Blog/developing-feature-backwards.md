Title: Developing a New Feature, Backwards
Date: 2015-02-10
==
If you've mastered the basics of TDD, it is easy to add a new piece of functionality to an existing feature using it. For example, to add an edge case to a calculator. Or to add a welcome email to the registration process. All it takes is writing a new test for an existing method, or for a new method in an existing class, and, red-green-refactor, here we go.

Not so easy when you are starting to work on a new feature. You want to run to a blackboard and draw diagrams and layers of abstraction vigorously, but the shadows of TDD Grandfathers look at you with infinite sadness, so you suppress your urge and just stare at the screen, not knowing how to start writing your first test.

I'm going to tell you how I'm doing it, and been doing it for a few years, using a concrete example of a feature I'm developing right now. Don't treat me like a guru, check it and see if it works for you like it does for me. I'm not going to bother you with code samples this time, maybe a couple of lines of pseudocode here and there.

You're gonna like it.
--
In a Web application, a typical feature can be seen as a "dialogue" between a user and a Smart Machine. We programmers should program The Machine so that her answers is, well, smart. Many times we have several user actions related to a particular feature, and the most important is the last action. In my case, the feature I am talking about is choosing a new project from a template. First a user opens a new project screen, and the system, in response to this action, displays the list of available templates (and many other things, of course). This is Step 1. Next, the user chooses a template, and the system creates a new project, based on this choice and many other parameters entered at the New Project screen. This is Step 2, and it is the purpose of the feature we're developing.

We are tempted to start with Step 1. We're going to start with Step 2 instead. Here's why.

After we develop the server-side code for Step 2, we'll know what kind of input data is required. In our case it's the full path to the project template and to the project being created. Next, we figure the data that's needed to be sent from the client side (note that we're getting closer to Step 1). Apparently we don't want to leak the absolute paths, so we send relative paths instead. Then we get to Step1 client side. We see that we have to have the relative template paths on the client (in addition to their names and, possibly, descriptions and icons, for UI purposes). Now we're ready to write the code that displays the list of templates and sends the user's choice to the server. Finally, we know what kind of info we need to get from the server before displaying the list of templates to the user, so we are ready to develop the server-side part of Step 1.  If you have client-side and server-side testing set up, the entire feature can be perfected before you open it in your browser for the first time. 

Unless you want to make it good looking, too.