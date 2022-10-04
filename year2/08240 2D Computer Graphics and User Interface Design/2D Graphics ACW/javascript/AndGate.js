var AndGate = (function (_super) 
{
	__extends(AndGate, _super);
    function AndGate(pPosition) 
	{
		_super.call(this);
    }
    
	AndGate.prototype.drawShape = function (context) 
	{
		//alert("and gate");
		

		context.moveTo(-50,50);
		context.lineTo(-50,100);
		context.lineTo(50,100);
		context.lineTo(50,50);
		context.arc(0,50,50,0,Math.PI,true);
		context.moveTo(0,0);
		context.lineTo(0,-25);
		context.moveTo(0,100);
		context.lineTo(0,125);
		context.stroke();
    };
	
    
	
    return AndGate;
})(FaultTreeGate);
