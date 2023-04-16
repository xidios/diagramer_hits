SignalRCalls = function (graph, signalRConnection, roomId) {
    this.graph = graph;
    this.signalRConnection = signalRConnection;
    this.roomId = roomId;

    this.addSignalRCallsToModel = function () {
        this.model = this.graph.getModel();
        this.model.addSignalRCalls(this);
        this.graph.connectionHandler.addSignalRCalls(this);
    }
    this.addSignalRCallsToModel();

    this.addCellsToData = function (data, cells) {
        if (cells == null) {
            return;
        }
        cells.sort((a, b) => b.isVertex() - a.isVertex());
        for (var i = 0; i < cells.length; i++) {
            var cellData = {
                children: []
            }
            var value = cells[i].getValue();
            var style = cells[i].getStyle();
            var geometry = cells[i].getGeometry();
            var id = cells[i].getId()
            this.addCellsToData(cellData.children, cells[i].children)
            cellData.geometry = {
                cellId: cells[i].id,
                x: geometry.x,
                y: geometry.y,
                width: geometry.width,
                height: geometry.height,
                relative: geometry.relative,
                offset: cells[i].geometry.offset ? {
                    x: cells[i].geometry.offset.x,
                    y: cells[i].geometry.offset.y
                } : null,
                sourcePoint: geometry.sourcePoint ? {x: geometry.sourcePoint.x, y: geometry.sourcePoint.y} : null,
                targetPoint: geometry.targetPoint ? {x: geometry.targetPoint.x, y: geometry.targetPoint.y} : null,
                points: []
            };
            cellData.id = id;
            cellData.value = value;
            cellData.style = style;
            cellData.isVertex = cells[i].isVertex();
            cellData.isEdge = cells[i].isEdge();
            cellData.parentId = cells[i].parent ? cells[i].parent.id : null;
            if (cellData.isVertex) {
                data.push(cellData);
                continue;
            }
            cellData.sourceId = cells[i].source ? cells[i].source.id : null;
            cellData.targetId = cells[i].target ? cells[i].target.id : null;
            if (geometry.points) {
                for (var p = 0; p < geometry.points.length; p++) {
                    var point = geometry.points[p];
                    cellData.geometry.points.push({x: point.x, y: point.y});
                }
            }
            data.push(cellData);
        }
    }

    this.graph.addListener(mxEvent.CELLS_ADDED, function (sender, evt) {
        var cells = evt.getProperty('cells');
        var data = [];
        this.signalRCalls.addCellsToData(data, cells);

        if (this.signalRCalls.signalRConnection != null && data.length > 0) {
            this.signalRCalls.signalRConnection.invoke("AddCellsOnDiagram", JSON.stringify(data), this.signalRCalls.roomId).then(() => {
                console.log("AddCellsOnDiagram send");
            });
        }
    });
    this.model.addListener(mxEvent.CHANGE, function (sender, evt) {
        var changes = evt.getProperty('edit').changes;
        if (this.signalRCalls.signalRConnection != null) {
            for (var i = 0; i < changes.length; i++) {
                if (!changes[i].isSignalRCall) {
                    if (changes[i] instanceof mxGeometryChange) {
                        var cell = changes[i].cell;
                        var data = {};
                        if (cell.isVertex()) {
                            var geometry = changes[i].geometry;
                            data = {
                                cellType: "vertex",
                                cellId: cell.id,
                                x: geometry.x,
                                y: geometry.y,
                                width: geometry.width,
                                height: geometry.height,
                                offset: geometry.offset ? {x: geometry.offset.x, y: geometry.offset.y} : null
                            }
                        } else if (cell.isEdge()) {
                            var geometry = changes[i].geometry;
                            data = {
                                cellType: "edge",
                                cellId: cell.id,
                                x: geometry.x,
                                y: geometry.y,
                                width: geometry.width,
                                height: geometry.height,
                                points: [],
                                sourcePoint: null,
                                targetPoint: null,
                                offset: geometry.offset ? {x: geometry.offset.x, y: geometry.offset.y} : null
                            }
                            if (geometry.sourcePoint) {
                                data.sourcePoint = {x: geometry.sourcePoint.x, y: geometry.sourcePoint.y}
                            }
                            if (geometry.targetPoint) {
                                data.targetPoint = {x: geometry.targetPoint.x, y: geometry.targetPoint.y}
                            }
                            if (geometry.points) {
                                for (var p = 0; p < geometry.points.length; p++) {
                                    var point = geometry.points[p];
                                    data.points.push({x: point.x, y: point.y});
                                }
                            }
                        }

                        this.signalRCalls.signalRConnection.invoke("MxGeometryChange", JSON.stringify(data), this.signalRCalls.roomId).then(() => {
                            console.log("MxGeometryChange send");
                        });
                    } else if (changes[i] instanceof mxTerminalChange) {
                        var data = {
                            cellId: changes[i].cell.id,
                            source: changes[i].source, // проверяется какая именно точка изменяется - начальная или конечная
                            terminalId: changes[i].terminal ? changes[i].terminal.id : null,
                        }
                        if (changes[i].previous == null && changes[i].terminal == null) {
                            continue
                        }
                        this.signalRCalls.signalRConnection.invoke("MxTerminalChange", JSON.stringify(data), this.signalRCalls.roomId).then(() => {
                            console.log("MxTerminalChange send");
                        });
                    } else if (changes[i] instanceof mxStyleChange) {
                        var data = {
                            cellId: changes[i].cell.id,
                            style: changes[i].style
                        }
                        this.signalRCalls.signalRConnection.invoke("MxStyleChange", JSON.stringify(data), this.signalRCalls.roomId).then(() => {
                            console.log("MxStyleChangeChange send");
                        });
                    } else if (changes[i] instanceof mxChildChange) {
                        var data = {
                            childId: changes[i].child.id,
                            parentId: changes[i].parent ? changes[i].parent.id : null,
                            index: changes[i].index
                        }
                        this.signalRCalls.signalRConnection.invoke("MxChildChange", JSON.stringify(data), this.signalRCalls.roomId).then(() => {
                            console.log("MxChildChange send");
                        });
                    } else if (changes[i] instanceof mxValueChange) {
                        var data = {
                            cellId: changes[i].cell.id,
                            value: changes[i].value
                        }
                        this.signalRCalls.signalRConnection.invoke("MxValueChange", JSON.stringify(data), this.signalRCalls.roomId).then(() => {
                            console.log("MxValueChange send");
                        });
                    } else if (changes[i] instanceof mxCollapseChange) {
                        var data = {
                            cellId: changes[i].cell.id,
                            collapsed: changes[i].collapsed

                        }
                        this.signalRCalls.signalRConnection.invoke("MxCollapseChange", JSON.stringify(data), this.signalRCalls.roomId).then(() => {
                            console.log("MxCollapseChange send");
                        });
                    }
                }
            }
        }

    });
    this.graph.connectionHandler.addListener(mxEvent.CONNECT, function (sender, evt) {
        var edge = evt.getProperty('cell');
        var source = edge.source;
        var target = edge.target;
        var data = {
            id: edge.id,
            style: edge.style,
            parentId: edge.parent.id,
            sourceId: source.id,
            targetId: target ? target.id : null,
            geometry: {
                targetPoint: edge.geometry.targetPoint ? {x: edge.geometry.targetPoint.x, y: edge.geometry.targetPoint.y} : null
            },
            points : [],
            isEdge : true,
            value : edge.value
        }
        if (this.signalRCalls.signalRConnection != null) {
            this.signalRCalls.signalRConnection.invoke("AddEdgeOnDiagram", JSON.stringify(data), this.signalRCalls.roomId).then(() => {
                console.log("AddEdgeOnDiagram");
            });
        }
    });
}

