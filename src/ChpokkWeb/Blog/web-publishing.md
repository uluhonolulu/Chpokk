Title: How to create a Web online and preview it on Azure or AppHarbor using our cloud IDE
Date: 2015-08-07T22:59:21
==
Ever since the introduction of Project Templates, creating a Web site online using Chpokk became as easy as it was in your favorite desktop .Net IDE (no, i don't mean Sublime). However, every user wants to see their beloved creation at some point, and do so every other minute. Now, *this* wasn't so obvious, until now.
--

I must also say that, while making a console program run in your browser is relatively simple, previewing a Web would require my online IDE to do some system-related stuff that my hoster (currently AppHarbor) just won't let me do. So I decided to take a shortcut and let you publish the Web you created to Azure or AppHarbor (or to another remote repository of your choice).

Here's how you do it.

Create a Website on Microsoft Azure. Make sure you have an account, then go to [the Azure dashboard](https://manage.windowsazure.com/)
![Azure create Website, step 1](/_content/images/help/azure1.png)

![Azure create Website, step 2](/_content/images/help/azure2.png)

![Azure create Website, step 3](/_content/images/help/azure3.png)

(note the Web's name, it will be used when we enter the publishing details in Chpokk)

Setup deployment from a local Git repository. In our case, "local" means the one that is hosted by Chpokk.
![Azure setup publishing, step 1](/_content/images/help/azure4.png)

![Azure setup publishing, step 2](/_content/images/help/azure5.png)

![Azure setup publishing, step 3](/_content/images/help/azure6.png)

or..

Create a Website on AppHarbor. Make sure you have an account, then go to [the AppHarbor's Applications page](https://appharbor.com/applications)
![AppHarbor create a Website](/_content/images/help/apphb.png)

Now go to Chpokk, open (or create) a Web project, and open a file for editing.

![Chpokk, create a Web project](/_content/images/help/chpokk_create_web.png)

Click the "Preview your Web" button above the code editor.
![Click that button](/_content/images/help/chpokk_pub_button.png)
* Use the name of the Web site you created on Step 1. 
* Enter the credentials that you use for logging into Azure or AppHarbor (don't worry, I don't store them anywhere, they go straight to the remote server).
* Click "Publish"
![Setup publishing](/_content/images/help/chpokk_pub_dialog.png)

A few things will happen. First, our cloud IDE will initialize a Git repository for you (if it wasn't there already). It will also perform the first commit, named "Initial commit". Finally, it will push the code to the target repository for you. The target repository will compile, run tests etc etc, and finally will make your Website visible for the rest of the world. Although Chpokk will display an URL which would let you preview the site, the changes won't be there for some time -- it takes a while (from a few seconds to a minute) for Azure to publish the changes.

![Published](/_content/images/help/chpokk_pub_result.png)

After the page's been reloaded, the button says "Publish to Azure", and the dialog just requires your credentials (which are supposed tobe remembered by your browser):

![Reloaded](/_content/images/help/chpokk_reloaded.png)

### What did we do here
In this walkthrough, we have created a simple (empty) Web project online and published it to a free Azure Website (Appharbor was covered too). All this without installing anything on your computer, so it works with any reasonably modern browser on any device. Yes, you **can** develop Ap.Net websites on your iPhone now.

This is it for today. If you want another cloud hosting service integrated, please let me know.

