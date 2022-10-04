var FaultTreeNode = (function () 
{
    function FaultTreeNode() 
	{
        this.setId(FaultTreeNode.sTotalNodes++);
    }
	FaultTreeNode.sTotalNodes = 0;
    FaultTreeNode.prototype.getId = function () 
	{
        return this.mId;
    };
	
    FaultTreeNode.prototype.setId = function (pId) 
	{
        this.mId = pId;
    };

    FaultTreeNode.prototype.getWidth = function()
    {
        //function to find the whole width of a tree
    	var width = 0;

    	if (this instanceof FaultTreeGate)
    	{
    		for (var i = 0; i < this.numChildren(); i++)
    		{
    			width += this.getChild(i).getWidth();
    		}
    	}
    	else
    	{
    		width = 100;
    	}
    	return width; 
    }

    FaultTreeNode.prototype.getHeight = function()
    {
        //function to find the whole width of a tree
    	var height = 150;

    	if (this instanceof FaultTreeGate)
    	{
    		var childHeight = 0;
    		for (var i = 0; i < this.numChildren(); i++)
    		{
    			if (this.getChild(i).getHeight() > childHeight)
    			{
    				childHeight = this.getChild(i).getHeight();
    			}
    		}
    		height += childHeight;
    	}
    	return height; 
    }
	
	FaultTreeNode.prototype.draw = function (context,position) 
	{
		context.translate(position.getX(),position.getY());
		context.beginPath();
		this.drawShape(context);
		context.closePath();
		if (this instanceof FaultTreeGate)
		{
			context.beginPath();
			context.moveTo(-(this.getWidth()/2) + (this.getChild(0).getWidth()/2) , 125);
			context.lineTo((this.getWidth()/2) - (this.getChild(this.numChildren()-1).getWidth()/2) , 125);
			context.closePath();
			context.stroke();

			var offset = 0;
			for (var i = 0; i < this.numChildren(); i++)
			{
				var x = -(this.getWidth()/2) + (this.getChild(i).getWidth()/2) + offset;
				this.getChild(i).draw(context,new Vector(x,150));
				offset += this.getChild(i).getWidth();
			}

		}
		context.translate(-position.getX(),-position.getY());	
    };
	
    return FaultTreeNode;
})();
