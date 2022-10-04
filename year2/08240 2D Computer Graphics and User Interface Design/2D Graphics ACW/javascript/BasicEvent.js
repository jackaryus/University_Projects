var BasicEvent = (function (_super) 
{
	__extends(BasicEvent, _super);
    function BasicEvent(pPosition) 
	{
		_super.call(this);
    };
	
    BasicEvent.prototype.drawShape = function (context) 
	{
		//alert("basic event");

		var numSegments = 20;
		var radius = 50;
		var anglePerSegment = Math.PI * 2 / numSegments;

		for(var i = 0; i <= numSegments; i++)
		{
			var angle = anglePerSegment * i;
			var x = radius * Math.cos(angle);
			var y = 50 + radius * Math.sin(angle);
			if (i == 0)
			{
				context.moveTo(x,y);
			}
			else
			{
				context.lineTo(x,y);
			}
		}
		context.moveTo(0,0);
		context.lineTo(0,-25);
		context.stroke();
    };
	

    return BasicEvent;
})(FaultTreeNode);
