(function () {
    "use strict";

    var list = new WinJS.Binding.List();
    var groupedItems = list.createGrouped(
        function groupKeySelector(item) { return item.group.key; },
        function groupDataSelector(item) { return item.group; }
    );

    // TODO: remplacez les données par vos vraies données.
    // Vous pouvez ajouter des données de sources asynchrones chaque fois qu'elles deviennent disponibles.
    generateSampleData().forEach(function (item) {
        list.push(item);
    });

    WinJS.Namespace.define("Data", {
        items: groupedItems,
        groups: groupedItems.groups,
        getItemReference: getItemReference,
        getItemsFromGroup: getItemsFromGroup,
        resolveGroupReference: resolveGroupReference,
        resolveItemReference: resolveItemReference
    });

    // Obtenez une référence pour un élément, en utilisant la clé de groupe et le titre d'élément, sous forme de
    // référence unique à l'élément facile à sérialiser.
    function getItemReference(item) {
        return [item.group.key, item.title];
    }

    // Cette fonction retourne un élément WinJS.Binding.List contenant uniquement les éléments
    // qui appartiennent au groupe fourni.
    function getItemsFromGroup(group) {
        return list.createFiltered(function (item) { return item.group.key === group.key; });
    }

    // Obtient l'unique groupe correspondant à la clé de groupe fournie.
    function resolveGroupReference(key) {
        for (var i = 0; i < groupedItems.groups.length; i++) {
            if (groupedItems.groups.getAt(i).key === key) {
                return groupedItems.groups.getAt(i);
            }
        }
    }

    // Obtient un élément unique à partir du tableau de chaînes fourni. Il doit contenir une
    // clé de groupe et un titre d'élément.
    function resolveItemReference(reference) {
        for (var i = 0; i < groupedItems.length; i++) {
            var item = groupedItems.getAt(i);
            if (item.group.key === reference[0] && item.title === reference[1]) {
                return item;
            }
        }
    }

    // Retourne un tableau d'exemples de données qui peuvent être ajoutées à la liste de données
    // de l'application. 
    function generateSampleData() {
        var itemContent = "<p>Curabitur class aliquam vestibulum nam curae maecenas sed integer cras phasellus suspendisse quisque donec dis praesent accumsan bibendum pellentesque condimentum adipiscing etiam consequat vivamus dictumst aliquam duis convallis scelerisque est parturient ullamcorper aliquet fusce suspendisse nunc hac eleifend amet blandit facilisi condimentum commodo scelerisque faucibus aenean ullamcorper ante mauris dignissim consectetuer nullam lorem vestibulum habitant conubia elementum pellentesque morbi facilisis arcu sollicitudin diam cubilia aptent vestibulum auctor eget dapibus pellentesque inceptos leo egestas interdum nulla consectetuer suspendisse adipiscing pellentesque proin lobortis sollicitudin augue elit mus congue fermentum parturient fringilla euismod feugiat</p><p>Curabitur class aliquam vestibulum nam curae maecenas sed integer cras phasellus suspendisse quisque donec dis praesent accumsan bibendum pellentesque condimentum adipiscing etiam consequat vivamus dictumst aliquam duis convallis scelerisque est parturient ullamcorper aliquet fusce suspendisse nunc hac eleifend amet blandit facilisi condimentum commodo scelerisque faucibus aenean ullamcorper ante mauris dignissim consectetuer nullam lorem vestibulum habitant conubia elementum pellentesque morbi facilisis arcu sollicitudin diam cubilia aptent vestibulum auctor eget dapibus pellentesque inceptos leo egestas interdum nulla consectetuer suspendisse adipiscing pellentesque proin lobortis sollicitudin augue elit mus congue fermentum parturient fringilla euismod feugiat</p><p>Curabitur class aliquam vestibulum nam curae maecenas sed integer cras phasellus suspendisse quisque donec dis praesent accumsan bibendum pellentesque condimentum adipiscing etiam consequat vivamus dictumst aliquam duis convallis scelerisque est parturient ullamcorper aliquet fusce suspendisse nunc hac eleifend amet blandit facilisi condimentum commodo scelerisque faucibus aenean ullamcorper ante mauris dignissim consectetuer nullam lorem vestibulum habitant conubia elementum pellentesque morbi facilisis arcu sollicitudin diam cubilia aptent vestibulum auctor eget dapibus pellentesque inceptos leo egestas interdum nulla consectetuer suspendisse adipiscing pellentesque proin lobortis sollicitudin augue elit mus congue fermentum parturient fringilla euismod feugiat</p><p>Curabitur class aliquam vestibulum nam curae maecenas sed integer cras phasellus suspendisse quisque donec dis praesent accumsan bibendum pellentesque condimentum adipiscing etiam consequat vivamus dictumst aliquam duis convallis scelerisque est parturient ullamcorper aliquet fusce suspendisse nunc hac eleifend amet blandit facilisi condimentum commodo scelerisque faucibus aenean ullamcorper ante mauris dignissim consectetuer nullam lorem vestibulum habitant conubia elementum pellentesque morbi facilisis arcu sollicitudin diam cubilia aptent vestibulum auctor eget dapibus pellentesque inceptos leo egestas interdum nulla consectetuer suspendisse adipiscing pellentesque proin lobortis sollicitudin augue elit mus congue fermentum parturient fringilla euismod feugiat</p><p>Curabitur class aliquam vestibulum nam curae maecenas sed integer cras phasellus suspendisse quisque donec dis praesent accumsan bibendum pellentesque condimentum adipiscing etiam consequat vivamus dictumst aliquam duis convallis scelerisque est parturient ullamcorper aliquet fusce suspendisse nunc hac eleifend amet blandit facilisi condimentum commodo scelerisque faucibus aenean ullamcorper ante mauris dignissim consectetuer nullam lorem vestibulum habitant conubia elementum pellentesque morbi facilisis arcu sollicitudin diam cubilia aptent vestibulum auctor eget dapibus pellentesque inceptos leo egestas interdum nulla consectetuer suspendisse adipiscing pellentesque proin lobortis sollicitudin augue elit mus congue fermentum parturient fringilla euismod feugiat</p><p>Curabitur class aliquam vestibulum nam curae maecenas sed integer cras phasellus suspendisse quisque donec dis praesent accumsan bibendum pellentesque condimentum adipiscing etiam consequat vivamus dictumst aliquam duis convallis scelerisque est parturient ullamcorper aliquet fusce suspendisse nunc hac eleifend amet blandit facilisi condimentum commodo scelerisque faucibus aenean ullamcorper ante mauris dignissim consectetuer nullam lorem vestibulum habitant conubia elementum pellentesque morbi facilisis arcu sollicitudin diam cubilia aptent vestibulum auctor eget dapibus pellentesque inceptos leo egestas interdum nulla consectetuer suspendisse adipiscing pellentesque proin lobortis sollicitudin augue elit mus congue fermentum parturient fringilla euismod feugiat</p><p>Curabitur class aliquam vestibulum nam curae maecenas sed integer cras phasellus suspendisse quisque donec dis praesent accumsan bibendum pellentesque condimentum adipiscing etiam consequat vivamus dictumst aliquam duis convallis scelerisque est parturient ullamcorper aliquet fusce suspendisse nunc hac eleifend amet blandit facilisi condimentum commodo scelerisque faucibus aenean ullamcorper ante mauris dignissim consectetuer nullam lorem vestibulum habitant conubia elementum pellentesque morbi facilisis arcu sollicitudin diam cubilia aptent vestibulum auctor eget dapibus pellentesque inceptos leo egestas interdum nulla consectetuer suspendisse adipiscing pellentesque proin lobortis sollicitudin augue elit mus congue fermentum parturient fringilla euismod feugiat";
        var itemDescription = "Item Description: Pellentesque porta mauris quis interdum vehicula urna sapien ultrices velit nec venenatis dui odio in augue cras posuere enim a cursus convallis neque turpis malesuada erat ut adipiscing neque tortor ac erat";
        var groupDescription = "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante";

        // Ces trois chaînes encodent des images d'espaces réservés. Vous définissez la
        // propriété backgroundImage dans vos données réelles comme des URL d'images.
        var darkGray = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsQAAA7EAZUrDhsAAAANSURBVBhXY3B0cPoPAANMAcOba1BlAAAAAElFTkSuQmCC";
        var lightGray = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsQAAA7EAZUrDhsAAAANSURBVBhXY7h4+cp/AAhpA3h+ANDKAAAAAElFTkSuQmCC";
        var mediumGray = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsQAAA7EAZUrDhsAAAANSURBVBhXY5g8dcZ/AAY/AsAlWFQ+AAAAAElFTkSuQmCC";

        // Chacun de ces exemples de groupes doit avoir une clé unique pour être affiché
        // séparément.
        var sampleGroups = [
            { key: "group1", title: "Group Title: 1", subtitle: "Group Subtitle: 1", backgroundImage: darkGray, description: groupDescription },
            { key: "group2", title: "Group Title: 2", subtitle: "Group Subtitle: 2", backgroundImage: lightGray, description: groupDescription },
            { key: "group3", title: "Group Title: 3", subtitle: "Group Subtitle: 3", backgroundImage: mediumGray, description: groupDescription },
            { key: "group4", title: "Group Title: 4", subtitle: "Group Subtitle: 4", backgroundImage: lightGray, description: groupDescription },
            { key: "group5", title: "Group Title: 5", subtitle: "Group Subtitle: 5", backgroundImage: mediumGray, description: groupDescription },
            { key: "group6", title: "Group Title: 6", subtitle: "Group Subtitle: 6", backgroundImage: darkGray, description: groupDescription }
        ];

        // Chacun de ces exemples d'éléments doit avoir une référence à un
        // groupe spécifique.
        var sampleItems = [
            { group: sampleGroups[0], title: "Item Title: 1", subtitle: "Item Subtitle: 1", description: itemDescription, content: itemContent, backgroundImage: lightGray },
            { group: sampleGroups[0], title: "Item Title: 2", subtitle: "Item Subtitle: 2", description: itemDescription, content: itemContent, backgroundImage: darkGray },
            { group: sampleGroups[0], title: "Item Title: 3", subtitle: "Item Subtitle: 3", description: itemDescription, content: itemContent, backgroundImage: mediumGray },
            { group: sampleGroups[0], title: "Item Title: 4", subtitle: "Item Subtitle: 4", description: itemDescription, content: itemContent, backgroundImage: darkGray },
            { group: sampleGroups[0], title: "Item Title: 5", subtitle: "Item Subtitle: 5", description: itemDescription, content: itemContent, backgroundImage: mediumGray },

            { group: sampleGroups[1], title: "Item Title: 1", subtitle: "Item Subtitle: 1", description: itemDescription, content: itemContent, backgroundImage: darkGray },
            { group: sampleGroups[1], title: "Item Title: 2", subtitle: "Item Subtitle: 2", description: itemDescription, content: itemContent, backgroundImage: mediumGray },
            { group: sampleGroups[1], title: "Item Title: 3", subtitle: "Item Subtitle: 3", description: itemDescription, content: itemContent, backgroundImage: lightGray },

            { group: sampleGroups[2], title: "Item Title: 1", subtitle: "Item Subtitle: 1", description: itemDescription, content: itemContent, backgroundImage: mediumGray },
            { group: sampleGroups[2], title: "Item Title: 2", subtitle: "Item Subtitle: 2", description: itemDescription, content: itemContent, backgroundImage: lightGray },
            { group: sampleGroups[2], title: "Item Title: 3", subtitle: "Item Subtitle: 3", description: itemDescription, content: itemContent, backgroundImage: darkGray },
            { group: sampleGroups[2], title: "Item Title: 4", subtitle: "Item Subtitle: 4", description: itemDescription, content: itemContent, backgroundImage: lightGray },
            { group: sampleGroups[2], title: "Item Title: 5", subtitle: "Item Subtitle: 5", description: itemDescription, content: itemContent, backgroundImage: mediumGray },
            { group: sampleGroups[2], title: "Item Title: 6", subtitle: "Item Subtitle: 6", description: itemDescription, content: itemContent, backgroundImage: darkGray },
            { group: sampleGroups[2], title: "Item Title: 7", subtitle: "Item Subtitle: 7", description: itemDescription, content: itemContent, backgroundImage: mediumGray },

            { group: sampleGroups[3], title: "Item Title: 1", subtitle: "Item Subtitle: 1", description: itemDescription, content: itemContent, backgroundImage: darkGray },
            { group: sampleGroups[3], title: "Item Title: 2", subtitle: "Item Subtitle: 2", description: itemDescription, content: itemContent, backgroundImage: lightGray },
            { group: sampleGroups[3], title: "Item Title: 3", subtitle: "Item Subtitle: 3", description: itemDescription, content: itemContent, backgroundImage: darkGray },
            { group: sampleGroups[3], title: "Item Title: 4", subtitle: "Item Subtitle: 4", description: itemDescription, content: itemContent, backgroundImage: lightGray },
            { group: sampleGroups[3], title: "Item Title: 5", subtitle: "Item Subtitle: 5", description: itemDescription, content: itemContent, backgroundImage: mediumGray },
            { group: sampleGroups[3], title: "Item Title: 6", subtitle: "Item Subtitle: 6", description: itemDescription, content: itemContent, backgroundImage: lightGray },

            { group: sampleGroups[4], title: "Item Title: 1", subtitle: "Item Subtitle: 1", description: itemDescription, content: itemContent, backgroundImage: lightGray },
            { group: sampleGroups[4], title: "Item Title: 2", subtitle: "Item Subtitle: 2", description: itemDescription, content: itemContent, backgroundImage: darkGray },
            { group: sampleGroups[4], title: "Item Title: 3", subtitle: "Item Subtitle: 3", description: itemDescription, content: itemContent, backgroundImage: lightGray },
            { group: sampleGroups[4], title: "Item Title: 4", subtitle: "Item Subtitle: 4", description: itemDescription, content: itemContent, backgroundImage: mediumGray },

            { group: sampleGroups[5], title: "Item Title: 1", subtitle: "Item Subtitle: 1", description: itemDescription, content: itemContent, backgroundImage: lightGray },
            { group: sampleGroups[5], title: "Item Title: 2", subtitle: "Item Subtitle: 2", description: itemDescription, content: itemContent, backgroundImage: darkGray },
            { group: sampleGroups[5], title: "Item Title: 3", subtitle: "Item Subtitle: 3", description: itemDescription, content: itemContent, backgroundImage: mediumGray },
            { group: sampleGroups[5], title: "Item Title: 4", subtitle: "Item Subtitle: 4", description: itemDescription, content: itemContent, backgroundImage: darkGray },
            { group: sampleGroups[5], title: "Item Title: 5", subtitle: "Item Subtitle: 5", description: itemDescription, content: itemContent, backgroundImage: lightGray },
            { group: sampleGroups[5], title: "Item Title: 6", subtitle: "Item Subtitle: 6", description: itemDescription, content: itemContent, backgroundImage: mediumGray },
            { group: sampleGroups[5], title: "Item Title: 7", subtitle: "Item Subtitle: 7", description: itemDescription, content: itemContent, backgroundImage: darkGray },
            { group: sampleGroups[5], title: "Item Title: 8", subtitle: "Item Subtitle: 8", description: itemDescription, content: itemContent, backgroundImage: lightGray }
        ];

        return sampleItems;
    }
})();
