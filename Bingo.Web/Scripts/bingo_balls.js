//=====================================
// Ball Container Variables
//=====================================

// jquery canvas object and canvas context
var $canvas, context;
// height and width of canvas
var height, width;
	
// default radius of the balls
var radius = 30;
	
// [textcolor,backgroundcolor]
var colors = [["#ffffff", "#014a81"]];
	
// frame rate variables
var fps = 75;
var timeInterval = 1000/fps;

// box2d variables
var worldAABB, world;
var gravity = new b2Vec2(0,250);
var doSleep = true;

// keep track of the balls
//var numbers = new Array(70);	// 70 is the max number of balls i expect
var numbers = new Array(100);
var seenNumbers = new Array();
var ballColors = new Array(70);
var balls = new Array();



//=====================================
// Public Functions
//=====================================
// create a new ball
function createBall(letter, number, initialize) {

    if (initialize == null || initialize == undefined) initialize = false;

	var bingoNumber = letter + number;

	animateDisplayBall(bingoNumber, initialize);

	var x = (Math.random() * 1000) % $canvas.width();
	if (x < 20) x += 20;

	var y = 0;

	//var x = 10;
	//var y = 0;

	seenNumbers.push(bingoNumber);

	var ballSd = new b2CircleDef();
	ballSd.density = 1
	ballSd.radius = radius;
	ballSd.restitution = 0.8;
	ballSd.friction = 0;
	var ballBd = new b2BodyDef();
	ballBd.AddShape(ballSd);
	ballBd.position.Set(x, y);
	var ball = world.CreateBody(ballBd);

	balls.push(ball);
	numbers[ball.m_shapeList.m_proxyId] = bingoNumber; //getBingoNumber();

	//var colorTheme = colors[Math.floor(Math.random() * 1000) % 7];
	var colorTheme = colors[0];
	ballColors[ball.m_shapeList.m_proxyId] = colorTheme;
}
//=====================================
// Theoretically Private Functions
//=====================================

function initializePhysics() {
	$canvas = $("#canvas");
	context = $canvas[0].getContext("2d");

	width = $canvas[0].width;
	height = $canvas[0].height;

	var x = $canvas[0].width / 2;
	var y = $canvas[0].height * .2;

	createWorld();
	//createBall(x, y);
	drawWorld();

	setInterval(updateStage, timeInterval);
}

function getNewBalls() {

    // debug information
    $("#pollTime").html(extractTime(serverTime));
    // does this match expected behavior?

	if (isgameOver) {
	    
	    gameOver();
	    return;

	}

	if (seenNumbers.length < 75) {

	    $.get(getNextBallUrl, function (result) {

	        // game still goes on
	        if (!result.gameover && result.ball.Number > 0) {
	            var txt = result.ball.Letter + result.ball.Number;

	            if ($.inArray(txt, seenNumbers) == -1) {
	                createBall(result.ball.Letter, result.ball.Number);
	            }
	        }
	        else if (result.gameover) {
	            isgameOver = true;
	        }

	        // do nothing if ball is <= 0
	    });
	}
	else {
	    gameOver();
	}

}

function animateDisplayBall(txt, initialize) {

    // drop the old one
    var container = $("#new_ball_container");
    var ball = container.find("#new_ball");
    container.empty();

    // create the new one
    var newBall = $("<div>").attr("id", "new_ball");
    newBall.append($("<span>").attr("id", "number").html(txt));
    $("#new_ball_container").append(newBall);

    if (!initialize) {
        // play the sound
        var audio = $("#moo")[0];

        audio.play();
    }
}

// demo that creates 30 balls automatically
/*
function fillBalls(){
	if (balls.length < 30) {
		var xpos = (Math.random() * 1000) % 400;	
		var y = $canvas[0].height * .2;					
		//var xpos = $canvas[0].width - 20;
		//var y = 0;
			
		createBall(xpos,y);
		
		setTimeout(fillBalls, 1000);
	}
}
*/

// update each stage
function updateStage() {
	clearCanvas();
	updateBalls();
	drawWorld();
}
	
// clear the canvas
function clearCanvas(){
	context.clearRect(0,0,$canvas[0].width, $canvas[0].height);
}
	
// update the location of all the balls
function updateBalls() {
	
	var timeStep = 1.0/60;
	var iteration = 1;
	world.Step(timeStep, iteration);
}
	
// creates the physics world
function createWorld() {
	worldAABB = new b2AABB();
	worldAABB.minVertex.Set(-1000, -1000);
	worldAABB.maxVertex.Set(1000, 1000);
	world = new b2World(worldAABB, gravity, doSleep);
	createGround(world);
}
	
// creates the boundries
function createGround(world) {			
		
	// ground
	createBox(world, (-1) * width, height, 5000, 10);
		
	// left wall
	createBox(world, 0, 0, 10, 500);
		
	// right all
	createBox(world, $canvas[0].width , 0, 10, 500);
}
	
// creates the boundry boxes
function createBox(world, x, y, width, height) {
	
	var boxSd = new b2BoxDef();
	boxSd.extents.Set(width, height);
		
	var boxBd = new b2BodyDef();
	boxBd.AddShape(boxSd);
	boxBd.position.Set(x,y);
		
	world.CreateBody(boxBd);			
}
		
	
	
// draw all the balls
function drawWorld() {	
	
	for (var j = world.m_jointList; j; j = j.m_next) {
		drawJoint(j, context);
	}
	
	for (var b = world.m_bodyList; b; b = b.m_next) {
		for (var s = b.GetShapeList(); s != null; s = s.GetNext()) {
			drawShapes(s, context);
		}
	}
}
	
function drawShapes(shape) {

	switch(shape.m_type)
	{
	    case b2Shape.e_circleShape:
	        {
	            var pos = shape.m_position;
	            var r = shape.m_radius;

	            /*
	            started working on rotation, no dice here
	            var rotation = shape.m_body.m_rotation;

	            if (rotation > 0) {
	            context.save();
	            context.rotate(.15); 
	            }
	            */

	            var colorTheme = ballColors[shape.m_proxyId];

	            if (colorTheme == undefined) colorTheme = colors[0];

	            context.beginPath();
	            context.arc(pos.x, pos.y, r, 0, 2 * Math.PI, false);
	            // radial gradient
	            var grd = context.createRadialGradient(pos.x - 15, pos.y - 15, 5, pos.x - 15, pos.y - 15, 30);
	            grd.addColorStop(0, "#7efe1b"); // light green
	            grd.addColorStop(1, "#459e00"); // dark green
	            context.fillStyle = grd;
	            context.fill();

	            if (hasMickey) {
	                var ear_r = shape.m_radius / 2;

	                context.beginPath();
	                context.arc(pos.x + r, pos.y - 30, ear_r, 0, 2 * Math.PI, false);
	                context.fillStyle = "#459e00";
	                context.fill();

	                context.beginPath();
	                context.arc(pos.x - r, pos.y - 30, ear_r, 0, 2 * Math.PI, false);
	                context.fillStyle = "#459e00";
                    context.fill();
	            }

	            // add the number
	            var bingoNum = numbers[shape.m_proxyId];
	            var textx = bingoNum.length > 2 ? pos.x - 22 : pos.x - 15;
	            if (bingoNum[0] == "I") textx = textx + 5;
	            var texty = pos.y + 7;

	            context.fillStyle = colorTheme[0];
	            context.font = "bold 25px serif";
	            context.fillText(numbers[shape.m_proxyId], textx, texty);

	        }
	        break;
		case b2Shape.e_polyShape:
		{
			var poly = shape;
			var tV = b2Math.AddVV(poly.m_position, b2Math.b2MulMV(poly.m_R, poly.m_vertices[0]));
			context.moveTo(tV.x, tV.y);
			for (var i = 0; i < poly.m_vertexCount; i++) {
				var v = b2Math.AddVV(poly.m_position, b2Math.b2MulMV(poly.m_R, poly.m_vertices[i]));
				context.lineTo(v.x, v.y);
			}
			context.lineTo(tV.x, tV.y);
				
			context.strokeStyle = "#eee";
			context.stroke();
		}
		break;
	}
}