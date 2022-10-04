var TransferGate = (function (_super) 
{
	__extends(TransferGate, _super);
    function TransferGate(pPosition) 
	{
		_super.call(this);
    }
    TransferGate.prototype.drawShape = function (context) 
	{
		//alert("transfer gate");

		var numSegments = 3;
		var radius = 65;
		var anglePerSegment = Math.PI * 2 / numSegments;
		context.save();
		context.translate(0, 65)
		for(var i = 0; i <= numSegments; i++)
		{
			var angle = anglePerSegment * i;
			angle += Math.PI * 0.17;
			var x = radius * Math.cos(angle);
			var y = radius * Math.sin(angle);
			if (i == 0)
			{
				context.moveTo(x,y);
			}
			else
			{
				context.lineTo(x,y);
			}
		}
		context.moveTo(1,-65);
		context.lineTo(1,-90)
		context.stroke();
		context.restore();
    };
	
    return TransferGate;
})(FaultTreeNode);
