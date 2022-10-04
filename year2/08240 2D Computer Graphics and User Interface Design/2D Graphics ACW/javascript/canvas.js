//check to see if the browser supports the addEventListener function
if(window.addEventListener)
{
	window.addEventListener
	(
		'load', //this is the load event
		onLoad, //this is the event handler we are going to write
		false	//useCapture boolean value
	);
}

//the window load event handler
function onLoad()
{
	var canvas;
	var minimapCanvas;
	var minimapContext;
	var context;
	var exampleFaultTrees;
	var grabbed = false;
	var currentTree = 0;
	var zoom = 1;
	var mousePos;
	var prevMousePos;
	var canvasPos;
	var fullScreen = false;

	//this function will initialise our variables
	function initialise()
	{
		//find the canvas element using its id attribute.
		canvas = document.getElementById('canvas');
		//if it couldnt be found
		if (!canvas)
		{
			//make a message box pop up with the error.
			alert('Error: I cannot find the canvas element!');
			return;
		}
		//check if there is a get context function
		if (!canvas.getContext)
		{
			//make a message box pop up with the error.
			alert('Error: no canvas.getContext!');
			return;
		}
		//get the 2D canvas context
		context = canvas.getContext('2d');
		if (!context)
		{
			alert('Error: failed to getContext!');
			return;
		}
		
		if(canvas.addEventListener)
		{	
			//events
			window.addEventListener('keydown', onKeyDown, false);
			window.addEventListener('resize', onResize, false);
			canvas.addEventListener('mousewheel', onMouseWheel, false);
			canvas.addEventListener('mouseup', onMouseUp, false);
			canvas.addEventListener('mousedown', onMouseDown, false);
			canvas.addEventListener('mousemove', onMouseMove, false);
		}

		//tree initialisation
		exampleFaultTrees = new ExampleFaultTrees();
		// minimap initialisation
		minimapCanvas = document.createElement('canvas');
		minimapContext = minimapCanvas.getContext('2d');
		minimapCanvas.width = canvas.width / 4 ;
		var topNode = exampleFaultTrees.getFaultTree(currentTree);
		var ratioX = minimapCanvas.width/topNode.getWidth();
		minimapCanvas.height = topNode.getHeight() * ratioX;
		canvasPos = new Vector(0,0);
		mousePos = new Vector(0,0);
		prevMousePos = mousePos;
		fitToScreen();
	};

	function onKeyDown(event)
	{
		// increment tree index with right arrow key
		if (event.keyCode == 39) 
		{
			currentTree = currentTree < 2 ? currentTree + 1 : 0;
			fitToScreen();
			var topNode = exampleFaultTrees.getFaultTree(currentTree);
			var ratioX = minimapCanvas.width/topNode.getWidth();
			minimapCanvas.height = topNode.getHeight() * ratioX;
		}
		//decrement tree index with right arrow key
		else if (event.keyCode == 37)
		{
			currentTree = currentTree > 0 ? currentTree - 1 : 2;
			fitToScreen();
			var topNode = exampleFaultTrees.getFaultTree(currentTree);
			var ratioX = minimapCanvas.width/topNode.getWidth();
			minimapCanvas.height = topNode.getHeight() * ratioX;
		}
		//dynamic canvas on f key
		else if (event.keyCode == 70)
		{
			//if fullscreen go to set canvas and minimap size
			if (fullScreen)
			{
				canvas.width  = 1200;
  				canvas.height = 800;				
				fullScreen = false;
				minimapCanvas.width = canvas.width / 4 ;
				var topNode = exampleFaultTrees.getFaultTree(currentTree);
				var ratioX = minimapCanvas.width/topNode.getWidth();
				minimapCanvas.height = topNode.getHeight() * ratioX;
			}
			//if not fullscreen go to set canvas to window size and minimap accordingly
			else if (!fullScreen)
			{
				canvas.width = window.innerWidth - 25;
				canvas.height = window.innerHeight;
				fullScreen = true;
				minimapCanvas.width = canvas.width / 4 ;
				var topNode = exampleFaultTrees.getFaultTree(currentTree);
				var ratioX = minimapCanvas.width/topNode.getWidth();
				minimapCanvas.height = topNode.getHeight() * ratioX;
			}
		}
		//runs method when enter pressed
		else if (event.keyCode == 13)
		{
			fitToScreen();
		}
	};

	function onMouseUp(event)
	{
		grabbed = false;
	};

	function onMouseDown(event)
	{
		grabbed = true;

	};

	function onMouseMove(event)
	{	
		//finds mouse position
		prevMousePos = mousePos;
		mousePos = new Vector(event.clientX,event.clientY);

		//if the page has been grabbed then it will drag the image when mouse moves
		if (grabbed == true)
		{
			var drag = new Vector(mousePos.getX() - prevMousePos.getX() , mousePos.getY() - prevMousePos.getY());
			canvasPos.setX(canvasPos.getX() + drag.getX());
			canvasPos.setY(canvasPos.getY() + drag.getY());			
		}

	};
		
	function onMouseWheel(event)
	{
		//variable to hold the amount scrolled
		var scroll = event.wheelDelta / 3750;

		//sets the canvas position realative to the mouse and zooms in to a limit so the tree doesnt flip
		if (zoom + scroll >= 0.01 )
		{ 
			var relativeX = (mousePos.getX() - canvasPos.getX()) / zoom;
			var relativeY = (mousePos.getY() - canvasPos.getY()) / zoom;
			canvasPos.setX(canvasPos.getX() - (relativeX * scroll)); 
			canvasPos.setY(canvasPos.getY() - (relativeY * scroll));
			zoom += scroll;

		}

	};

	function onResize(event)
	{
		//if fullscreen then it allows you to resize the canvas dynamically with the window as the event triggers when the window is resized
		if (fullScreen)
		{
			canvas.width = window.innerWidth;
			canvas.height = window.innerHeight;
			minimapCanvas.width = canvas.width / 4 ;
			var topNode = exampleFaultTrees.getFaultTree(currentTree);
			var ratioX = minimapCanvas.width/topNode.getWidth();
			minimapCanvas.height = topNode.getHeight() * ratioX;
		};
	}

	function fitToScreen()
	{
		var topNode = exampleFaultTrees.getFaultTree(currentTree);
		//figures out the correct ratio/zoom to make the tree fit the screen
		var ratioX = canvas.width/topNode.getWidth();
		var ratioY = canvas.height/topNode.getHeight();
		//depending on which ratio is larger it uses the ratio and as the zoom so the tree fits and centres it
		if (ratioX < ratioY)
		{
			zoom = ratioX;
			canvasPos.setY((canvas.height / 2) - (topNode.getHeight()/2) * zoom);
			canvasPos.setX(0);
		}
	    else
	    {
	    	zoom = ratioY;
	    	canvasPos.setX((canvas.width / 2) - (topNode.getWidth()/2) * zoom);
	    	canvasPos.setY(0);
	    }

	}

	function drawloop()
	{
		context.save();
		draw();
		context.restore();
		window.setTimeout(drawloop,1000 / 60);
	};
	
	function draw()
	{
		//draw the tree to canvas and calls the draw minimap 
		context.fillStyle = "#BDBDBD";
		context.fillRect(0,0,canvas.width,canvas.height);
		context.save();
		context.translate(canvasPos.getX(),canvasPos.getY());
		context.scale(zoom, zoom);
		context.lineWidth = Math.max(4 / (zoom*10), 4);
		var topNode = exampleFaultTrees.getFaultTree(currentTree);
		topNode.draw(context,new Vector(topNode.getWidth()/2,25));
		context.restore();
		drawToMinimap();
		context.drawImage(minimapCanvas,canvas.width - minimapCanvas.width, 0);
	};

	function drawToMinimap()
	{
		//draw the tree to minimap
		minimapContext.fillStyle = "white";
		minimapContext.fillRect(0,0,minimapCanvas.width,minimapCanvas.height);
		minimapContext.save();
		minimapContext.translate(minimapCanvas.width/2,0);
		var topNode = exampleFaultTrees.getFaultTree(currentTree);
		var ratioX = minimapCanvas.width/topNode.getWidth();
		minimapContext.scale(ratioX,ratioX);
		topNode.draw(minimapContext,new Vector(0,25));
		minimapContext.restore();
		var mX = -canvasPos.getX() * (ratioX / zoom);
		var mY = -canvasPos.getY() * (ratioX / zoom);
		var mW = canvas.width * (ratioX / zoom);
		var mH = canvas.height * (ratioX / zoom);
		minimapContext.beginPath();
		minimapContext.rect(mX, mY, mW, mH);
		minimapContext.closePath();
		minimapContext.stroke();
	}
		
	//call the initialise and draw functions
	initialise();
	drawloop();
	
}
