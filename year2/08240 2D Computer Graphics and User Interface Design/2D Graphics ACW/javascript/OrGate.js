var OrGate = (function (_super) 
{
	__extends(OrGate, _super);
    function OrGate(pPosition) 
	{
		_super.call(this);
    }
    
	OrGate.prototype.drawShape = function (context) 
	{
		//alert("or gate");

		context.moveTo(0,0);
		context.quadraticCurveTo(-55,20,-50,80);
		context.quadraticCurveTo(0,50,50,80);
		context.quadraticCurveTo(55,20,0,0);
		context.moveTo(0,65);
		context.lineTo(0,125);
		context.moveTo(0,0);
		context.lineTo(0,-25)
		context.stroke();
    };
	
    
	
    return OrGate;
})(FaultTreeGate);
