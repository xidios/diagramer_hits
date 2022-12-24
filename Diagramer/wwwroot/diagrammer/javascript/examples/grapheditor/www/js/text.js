/**
 * Text extraction plugin.
 */
function textPlugin(editorUi)
{
    this.editorUi = editorUi;
    // Adds resource for action
    mxResources.parse('extractText=Extract Text...');

    // Adds action
    editorUi.actions.addAction('extractText', function()
    {
        var graph = editorUi.editor.graph;
        var text = graph.getIndexableText(
            (graph.isSelectionEmpty()) ? null :
                graph.getSelectionCells());
        var dlg = new EmbedDialog(editorUi, text, null,
            null, null, 'Extracted Text:');
        editorUi.showDialog(dlg.container, 450, 240, true, true);
        dlg.init();
    });

    var menu = editorUi.menus.get('extras');
    var oldFunct = menu.funct;

    menu.funct = function(menu, parent)
    {
        oldFunct.apply(this, arguments);

        editorUi.menus.addMenuItems(menu, ['-', 'extractText'], parent);
    };
}
