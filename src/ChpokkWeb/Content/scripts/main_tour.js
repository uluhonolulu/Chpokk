$(function () {
	var tour = new Tour({
		debug: false,
		name: 'main',
		backdrop: true,
		template: "<div class='popover tour'>          <div class='arrow'></div>          <h3 class='popover-title'></h3>          <div class='popover-content'><span/></div>          <nav class='popover-navigation'>            <div class='btn-group'>              <button class='btn btn-default' data-role='prev'>&laquo; Prev</button>              <button class='btn btn-default' data-role='next'>Next &raquo;</button>            </div>            <button class='btn btn-default' data-role='end'>End tour</button>          </nav>        </div>"
	});
	tour.addSteps([
				{
					element: '#brand',
					title: "Get started",
					content: "Chpokk needs your .Net code to work with. Several options are supported."
				},
				{
					element: "#cloneDialogOpener",
					title: "Clone a Git repository",
					content: "If your code is stored in a Git repository, clone it using this button.",
					placement: 'bottom'
				},
				{
					element: "#zipUploadInitiator",
					title: "Upload a zipped project",
					content: "If your code is stored on your device, zip it and upload it using this button.",
					placement: 'bottom'
				},
				{
					element: '#repositoryList',
					//title: "Get started",
					content: "If you have uploaded your code previously, you'll find it in this list."
				}
			]);
	tour.start();
})