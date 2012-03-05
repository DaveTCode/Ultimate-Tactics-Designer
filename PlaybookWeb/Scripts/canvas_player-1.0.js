<!--!=======================================================================-->
<!--! Canvas player class. Used to commonise functions and abstract the     -->
<!--! drawing away from the page on which it's drawn.                       -->
<!--!=======================================================================-->
function CanvasPlayer(canvas, xmlDoc)
{
    this.cycleTimeMs = 1000 / 60;
    this.isPlaying = false;
    this.isPaused = false;
    this.isStopped = false;
    
    var mPitchLength = 110;
    var mPitchWidth = 37;
    var mEndzoneDepth = 23;
    var mBrickDistance = 18;
    var mPlayerRadius = 0.75;
    var mDiscRadius = 0.4;
    var mCycleList = (xmlDoc == null) ? null : xmlDoc.getElementsByTagName("cycle");
    var mCycleIndex = 0;
    var mGraphicsContext = canvas.getContext("2d");
    var mRatio = conversionRatio(canvas.width, canvas.height);
    var mCanvasWidth = canvas.width;
    var mCanvasHeight = canvas.height;
    
    this.playNext = playNext;
    this.hasMore = hasMore;
    this.drawFirstFrame = drawFirstFrame;
    this.reset = reset;
    
    function reset()
    {
        mCycleIndex = 0;
    }
    
    <!--!===================================================================-->
    <!--! Returns true if there are more cycles still to render.            -->
    <!--!===================================================================-->
    function hasMore()
    {
        if (mCycleList != null && mCycleIndex < mCycleList.length)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    <!--!===================================================================-->
    <!--! Renders the next cycle in the sequence and then pauses sets a     -->
    <!--! timer to draw the following cycle (recursively).                  -->
    <!--!===================================================================-->
    function playNext()
    {        
        drawPitch();
        
        renderCycle(mCycleList[mCycleIndex])
        
        mCycleIndex+=4;
    }
    
    function drawFirstFrame()
    {
        drawPitch();
        
        if (mCycleList != null && mCycleList.length > 0)
        {
            renderCycle(mCycleList[0]);
        }
    }
    
    <!--!===================================================================-->
    <!--! Draws the pitch lines and clears the area.                        -->
    <!--!===================================================================-->
    function drawPitch()
    {            
        // Clear to the original pitch colour.
        mGraphicsContext.fillStyle = "rgb(0,60,0)";
        mGraphicsContext.fillRect(0, 0, mCanvasWidth, mCanvasHeight);
        
        // Draw outer pitch lines
        mGraphicsContext.moveTo(0, 0);
        mGraphicsContext.lineTo(0, mCanvasHeight);
        mGraphicsContext.lineTo(mCanvasWidth, mCanvasHeight);
        mGraphicsContext.lineTo(mCanvasWidth, 0);
        mGraphicsContext.lineTo(0, 0);
        
        // Draw inner pitch lines
        mGraphicsContext.moveTo(mEndzoneDepth * mRatio, 0);
        mGraphicsContext.lineTo(mEndzoneDepth * mRatio, mCanvasHeight);
        mGraphicsContext.moveTo(mCanvasWidth - mEndzoneDepth * mRatio, mCanvasHeight);
        mGraphicsContext.lineTo(mCanvasWidth - mEndzoneDepth * mRatio, 0);
        
        mGraphicsContext.strokeStyle = "rgb(255,255,255)";
        mGraphicsContext.stroke();
    }
    
    <!--!===================================================================-->
    <!--! Draw a single cycle onto the canvas.                              -->
    <!--!===================================================================-->
    function renderCycle(cycleXml)
    {
        var playerRadius = mRatio * mPlayerRadius;
        var discRadius = mRatio * mDiscRadius;
        
        var elements = cycleXml.childNodes;
        for (var ii = 0; ii < elements.length; ii++)
        {
            // Only interested in element nodes.
            if (elements[ii].nodeType == 1)
            {
                var x = elements[ii].getElementsByTagName("x")[0].firstChild.data;
                var y = elements[ii].getElementsByTagName("y")[0].firstChild.data;
                var radius = 0;
                
                if (elements[ii].nodeName == 'player')
                {
                    var team = elements[ii].attributes.getNamedItem("team").nodeValue;
                    
                    radius = playerRadius;
                    
                    if (team == "0")
                    {
                        mGraphicsContext.strokeStyle = "#FF0000";
                        mGraphicsContext.fillStyle = "#FF0000"
                    }
                    else if (team == "1")
                    {
                        mGraphicsContext.strokeStyle = "#0000FF";
                        mGraphicsContext.fillStyle = "#0000FF"
                    }
                }
                else if (elements[ii].nodeName == 'disc')
                {
                    mGraphicsContext.strokeStyle = "#FFFFFF";
                    mGraphicsContext.fillStyle = "#FFFFFF"
                    radius = discRadius;
                }
                
                mGraphicsContext.beginPath();
                mGraphicsContext.arc(y * mRatio, 
                                     x * mRatio, 
                                     playerRadius, 
                                     Math.PI * 2, 
                                     false);
                mGraphicsContext.closePath();
                
                mGraphicsContext.fill();
            }
        }
    }
    
    <!--!===================================================================-->
    <!--! Work out what the ratio should be for converting from pitch       -->
    <!--! coordinates into canvas coordinates. The value returned from this -->
    <!--! should be multiplied by the pitch coordinates.                    -->
    <!--!===================================================================-->
    function conversionRatio(canvasWidth, canvasHeight)
    {
        return canvasWidth / mPitchLength;
    }
}

<!--!=======================================================================-->
<!--! Plays the next frame in the canvas player and sets a timer to wait    -->
<!--! for the required number of ms before running the function again.      -->
<!--!=======================================================================-->
function playNextFrame(canvasPlayer, finishedCallback)
{
    if (canvasPlayer.isStopped)
    {
        return;
    }
    if (canvasPlayer.isPaused)
    {
        setTimeout(function() {playNextFrame(canvasPlayer, finishedCallback);}, 100);
        return;
    }
    
    canvasPlayer.isPlaying = true;

    var msAtStart = (new Date()).getTime();
    
    canvasPlayer.playNext();
    
    var executionLength = (new Date()).getTime() - msAtStart;
    
    var msToWait = canvasPlayer.cycleTimeMs - executionLength;
        
    if (msToWait > 0)
    {
        if (canvasPlayer.hasMore())
        {
            setTimeout(function() {playNextFrame(canvasPlayer, finishedCallback);}, msToWait);
        }
        else
        {
            finishedCallback();
        }
    }
    else
    {
        alert("Play is running too slowly. Unable to continue");
    }
}

function stop(canvasPlayer)
{
    canvasPlayer.isStopped = true;
    
    canvasPlayer.reset();
}

function pause(canvasPlayer)
{
    if (canvasPlayer.isPlaying)
    {
        canvasPlayer.isPaused = true;
        
        return true;
    }
    
    return false;
}

function play(canvasPlayer, finishedCallback)
{
    canvasPlayer.isPaused = false;
    canvasPlayer.isStopped = false;
    
    if (!canvasPlayer.isPlaying)
    {
        playNextFrame(canvasPlayer, finishedCallback);
        
        return true;
    }
    
    return false;
}

<!--!=======================================================================-->
<!--! Draw the first frame statically so that we have something to display  -->
<!--! to the user until they click the play button.                         -->
<!--!=======================================================================-->
function drawFirstFrame(canvasPlayer)
{
    canvasPlayer.drawFirstFrame();
}