$(function () {
	var tour = new Tour({
		debug: false,
		name: 'editor',
		backdrop: false,
		template: "<div class='popover tour'>          <div class='arrow'></div>          <h3 class='popover-title'></h3>          <div class='popover-content'><span/></div>          <nav class='popover-navigation'>            <div class='btn-group'>              <button class='btn btn-default' data-role='prev'>&laquo; Prev</button>              <button class='btn btn-default' data-role='next'>Next &raquo;</button>            </div>            <button class='btn btn-default' data-role='end'>End tour</button>          </nav>        </div>"
	});
	tour.addSteps([
				{
					element: '#saveButton',
					title: "Save your changes",
					content: "Click this button to save your changes.",
					placement: 'bottom'
				},
				{
					element: "#saveCommitButton",
					title: "Save your changes and commit them to the repository",
					content: "If your code is stored in a Git repository, commit it after saving using this button.",
					placement: 'bottom'
				},
				{
					element: "#parseButton",
					title: "Check your code for syntax errors",
					content: "Syntax check. Doesn't check for compile errors (e.g a mistyped variable), just for invalid language constructs (e.g. a missing bracket)",
					placement: 'bottom'
				},
				{
					element: '#newItemButton',
					title: "Create a new item",
					content: "Add a new code file to your project.",
					placement: 'bottom'
				},
				{
					element: '#compileButton',
					title: "Compile your project",
					content: "Compile the currently selected project (you must have an open file so that the system knows which project to compile).",
					placement: 'bottom'
				},
				{
					element: '#runButton',
					title: "Compile and run your project",
					content: "Compile and run the currently selected project, if it's a Console executable (you must have an open file so that the system knows which project to run).",
					placement: 'bottom'
				},
				{
					element: '#zipper',
					title: "Download your repository",
					content: "Download the content of your repository in a zip file."
				},
				{
					element: '#pusher',
					title: "Push your repository",
					content: "Push the committed changes back to origin."
				}
			]);
	tour.start();
})