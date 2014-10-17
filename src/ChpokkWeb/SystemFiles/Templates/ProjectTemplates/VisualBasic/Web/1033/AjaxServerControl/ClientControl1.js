/// <reference name="MicrosoftAjax.js"/>

Type.registerNamespace("$safeprojectname$");

$safeprojectname$.ClientControl1 = function(element) {
    $safeprojectname$.ClientControl1.initializeBase(this, [element]);
}

$safeprojectname$.ClientControl1.prototype = {
    initialize: function() {
        $safeprojectname$.ClientControl1.callBaseMethod(this, 'initialize');
        
        // Add custom initialization here
    },
    dispose: function() {        
        //Add custom dispose actions here
        $safeprojectname$.ClientControl1.callBaseMethod(this, 'dispose');
    }
}
$safeprojectname$.ClientControl1.registerClass('$safeprojectname$.ClientControl1', Sys.UI.Control);

if (typeof(Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();