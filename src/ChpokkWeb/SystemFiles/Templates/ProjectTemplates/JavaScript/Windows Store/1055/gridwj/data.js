(function () {
    "use strict";

    var list = new WinJS.Binding.List();
    var groupedItems = list.createGrouped(
        function groupKeySelector(item) { return item.group.key; },
        function groupDataSelector(item) { return item.group; }
    );

    // TODO: Veriyi, gerçek verilerinizle değiştirin.
    // Zaman uyumsuz kaynaklar kullanılabilir olduğunda, bu kaynaklardan veri ekleyebilirsiniz.
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

    // Grup anahtarını ve öğe başlığını kolayca seri haline getirilebilen
    // benzersiz bir başvuru olarak kullanarak öğe için bir başvuru alın.
    function getItemReference(item) {
        return [item.group.key, item.title];
    }

    // Bu işlev, yalnızca sağlanan gruba ait olan öğeleri içeren bir
    // WinJS.Binding.List döndürür.
    function getItemsFromGroup(group) {
        return list.createFiltered(function (item) { return item.group.key === group.key; });
    }

    // Sağlanan grup anahtarına karşılık gelen benzersiz grubu alın.
    function resolveGroupReference(key) {
        for (var i = 0; i < groupedItems.groups.length; i++) {
            if (groupedItems.groups.getAt(i).key === key) {
                return groupedItems.groups.getAt(i);
            }
        }
    }

    // Sağlanan dize dizisinden benzersiz bir öğe alın; bir
    // grup anahtarı ve bir öğe başlığı içermelidir.
    function resolveItemReference(reference) {
        for (var i = 0; i < groupedItems.length; i++) {
            var item = groupedItems.getAt(i);
            if (item.group.key === reference[0] && item.title === reference[1]) {
                return item;
            }
        }
    }

    // Uygulamanın veri listesine eklenebilecek örnek veriler içeren bir
    // dizi döndürür. 
    function generateSampleData() {
        var itemContent = "<p>Curabitur class aliquam vestibulum nam curae maecenas sed integer cras phasellus suspendisse quisque donec dis praesent accumsan bibendum pellentesque condimentum adipiscing etiam consequat vivamus dictumst aliquam duis convallis scelerisque est parturient ullamcorper aliquet fusce suspendisse nunc hac eleifend amet blandit facilisi condimentum commodo scelerisque faucibus aenean ullamcorper ante mauris dignissim consectetuer nullam lorem vestibulum habitant conubia elementum pellentesque morbi facilisis arcu sollicitudin diam cubilia aptent vestibulum auctor eget dapibus pellentesque inceptos leo egestas interdum nulla consectetuer suspendisse adipiscing pellentesque proin lobortis sollicitudin augue elit mus congue fermentum parturient fringilla euismod feugiat</p><p>Curabitur class aliquam vestibulum nam curae maecenas sed integer cras phasellus suspendisse quisque donec dis praesent accumsan bibendum pellentesque condimentum adipiscing etiam consequat vivamus dictumst aliquam duis convallis scelerisque est parturient ullamcorper aliquet fusce suspendisse nunc hac eleifend amet blandit facilisi condimentum commodo scelerisque faucibus aenean ullamcorper ante mauris dignissim consectetuer nullam lorem vestibulum habitant conubia elementum pellentesque morbi facilisis arcu sollicitudin diam cubilia aptent vestibulum auctor eget dapibus pellentesque inceptos leo egestas interdum nulla consectetuer suspendisse adipiscing pellentesque proin lobortis sollicitudin augue elit mus congue fermentum parturient fringilla euismod feugiat</p><p>Curabitur class aliquam vestibulum nam curae maecenas sed integer cras phasellus suspendisse quisque donec dis praesent accumsan bibendum pellentesque condimentum adipiscing etiam consequat vivamus dictumst aliquam duis convallis scelerisque est parturient ullamcorper aliquet fusce suspendisse nunc hac eleifend amet blandit facilisi condimentum commodo scelerisque faucibus aenean ullamcorper ante mauris dignissim consectetuer nullam lorem vestibulum habitant conubia elementum pellentesque morbi facilisis arcu sollicitudin diam cubilia aptent vestibulum auctor eget dapibus pellentesque inceptos leo egestas interdum nulla consectetuer suspendisse adipiscing pellentesque proin lobortis sollicitudin augue elit mus congue fermentum parturient fringilla euismod feugiat</p><p>Curabitur class aliquam vestibulum nam curae maecenas sed integer cras phasellus suspendisse quisque donec dis praesent accumsan bibendum pellentesque condimentum adipiscing etiam consequat vivamus dictumst aliquam duis convallis scelerisque est parturient ullamcorper aliquet fusce suspendisse nunc hac eleifend amet blandit facilisi condimentum commodo scelerisque faucibus aenean ullamcorper ante mauris dignissim consectetuer nullam lorem vestibulum habitant conubia elementum pellentesque morbi facilisis arcu sollicitudin diam cubilia aptent vestibulum auctor eget dapibus pellentesque inceptos leo egestas interdum nulla consectetuer suspendisse adipiscing pellentesque proin lobortis sollicitudin augue elit mus congue fermentum parturient fringilla euismod feugiat</p><p>Curabitur class aliquam vestibulum nam curae maecenas sed integer cras phasellus suspendisse quisque donec dis praesent accumsan bibendum pellentesque condimentum adipiscing etiam consequat vivamus dictumst aliquam duis convallis scelerisque est parturient ullamcorper aliquet fusce suspendisse nunc hac eleifend amet blandit facilisi condimentum commodo scelerisque faucibus aenean ullamcorper ante mauris dignissim consectetuer nullam lorem vestibulum habitant conubia elementum pellentesque morbi facilisis arcu sollicitudin diam cubilia aptent vestibulum auctor eget dapibus pellentesque inceptos leo egestas interdum nulla consectetuer suspendisse adipiscing pellentesque proin lobortis sollicitudin augue elit mus congue fermentum parturient fringilla euismod feugiat</p><p>Curabitur class aliquam vestibulum nam curae maecenas sed integer cras phasellus suspendisse quisque donec dis praesent accumsan bibendum pellentesque condimentum adipiscing etiam consequat vivamus dictumst aliquam duis convallis scelerisque est parturient ullamcorper aliquet fusce suspendisse nunc hac eleifend amet blandit facilisi condimentum commodo scelerisque faucibus aenean ullamcorper ante mauris dignissim consectetuer nullam lorem vestibulum habitant conubia elementum pellentesque morbi facilisis arcu sollicitudin diam cubilia aptent vestibulum auctor eget dapibus pellentesque inceptos leo egestas interdum nulla consectetuer suspendisse adipiscing pellentesque proin lobortis sollicitudin augue elit mus congue fermentum parturient fringilla euismod feugiat</p><p>Curabitur class aliquam vestibulum nam curae maecenas sed integer cras phasellus suspendisse quisque donec dis praesent accumsan bibendum pellentesque condimentum adipiscing etiam consequat vivamus dictumst aliquam duis convallis scelerisque est parturient ullamcorper aliquet fusce suspendisse nunc hac eleifend amet blandit facilisi condimentum commodo scelerisque faucibus aenean ullamcorper ante mauris dignissim consectetuer nullam lorem vestibulum habitant conubia elementum pellentesque morbi facilisis arcu sollicitudin diam cubilia aptent vestibulum auctor eget dapibus pellentesque inceptos leo egestas interdum nulla consectetuer suspendisse adipiscing pellentesque proin lobortis sollicitudin augue elit mus congue fermentum parturient fringilla euismod feugiat";
        var itemDescription = "Item Description: Pellentesque porta mauris quis interdum vehicula urna sapien ultrices velit nec venenatis dui odio in augue cras posuere enim a cursus convallis neque turpis malesuada erat ut adipiscing neque tortor ac erat";
        var groupDescription = "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante";

        // Bu üç dize yer tutucu resimlerini kodlar. Gerçek verinizdeki backgroundImage özelliğini
        // resimlere URL'leri yapmak için ayarlamak isteyebilirsiniz.
        var darkGray = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsQAAA7EAZUrDhsAAAANSURBVBhXY3B0cPoPAANMAcOba1BlAAAAAElFTkSuQmCC";
        var lightGray = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsQAAA7EAZUrDhsAAAANSURBVBhXY7h4+cp/AAhpA3h+ANDKAAAAAElFTkSuQmCC";
        var mediumGray = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsQAAA7EAZUrDhsAAAANSURBVBhXY5g8dcZ/AAY/AsAlWFQ+AAAAAElFTkSuQmCC";

        // Bu örnek grupların her birinin ayrı ayrı görüntülenmesi için bu gruplar benzersiz bir anahtara
        // sahip olmalıdır.
        var sampleGroups = [
            { key: "group1", title: "Group Title: 1", subtitle: "Group Subtitle: 1", backgroundImage: darkGray, description: groupDescription },
            { key: "group2", title: "Group Title: 2", subtitle: "Group Subtitle: 2", backgroundImage: lightGray, description: groupDescription },
            { key: "group3", title: "Group Title: 3", subtitle: "Group Subtitle: 3", backgroundImage: mediumGray, description: groupDescription },
            { key: "group4", title: "Group Title: 4", subtitle: "Group Subtitle: 4", backgroundImage: lightGray, description: groupDescription },
            { key: "group5", title: "Group Title: 5", subtitle: "Group Subtitle: 5", backgroundImage: mediumGray, description: groupDescription },
            { key: "group6", title: "Group Title: 6", subtitle: "Group Subtitle: 6", backgroundImage: darkGray, description: groupDescription }
        ];

        // Bu örnek öğelerin her biri belirli bir gruba başvuruya sahip
        // olmalıdır.
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
