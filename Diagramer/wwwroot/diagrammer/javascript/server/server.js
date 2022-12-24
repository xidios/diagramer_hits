const { emit } = require('process');
const { isStringObject } = require('util/types');
const http = require('http').createServer();
const io = require('socket.io')(http, {
    cors: { origin: "*" }
});

xml = `<mxGraphModel dx="495" dy="517" grid="1" gridSize="10" guides="1" tooltips="1" connect="1" arrows="1" fold="1" page="1" pageScale="1" pageWidth="827" pageHeight="1169">
  <root>
    <mxCell id="0" />
    <mxCell id="1" parent="0" />
    <mxCell id="2" value="Hello," parent="1" vertex="1">
      <mxGeometry x="20" y="20" width="80" height="30" as="geometry" />
      <CustomData as="data">
        {&quot;value&quot;:&quot;v1&quot;}
      </CustomData>
    </mxCell>
    <mxCell id="3" value="World!" parent="1" vertex="1">
      <mxGeometry x="290" y="570" width="80" height="30" as="geometry" />
      <CustomData as="data">
        {&quot;value&quot;:&quot;v2&quot;}
      </CustomData>
    </mxCell>
    <mxCell id="4" value="" parent="1" source="2" target="3" edge="1">
      <mxGeometry relative="1" as="geometry" />
    </mxCell>
  </root>
</mxGraphModel>
`
graph = {};
console.log();

io.on('connection', (socket) => {
    console.log('a user connected');
    io.emit('connection',xml);
    socket.on('message', (message) =>     {
        console.log(message);
        io.emit('message', `${socket.id.substr(0,2)} said ${message}` );   
    });

    socket.on("disconnect", (reason) => {
        console.log("User disconnected: " + reason); // undefined
      });

    socket.on("update_graph", (graph)=>{
      console.log("update graph");
      xml = graph;
      io.emit("update_graph",graph);
    });
});

http.listen(8080, () => console.log('listening on http://localhost:8080') );



