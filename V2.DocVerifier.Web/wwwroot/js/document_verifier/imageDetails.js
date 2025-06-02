var canvas = document.getElementById('myCanvas');
var context = canvas.getContext('2d');
const _img = new Image();

function loadImage(imageWidth, imageHeight, imageContent)
{    
    //const zoom = 0.7;
    const pageWidth = screen.availWidth;
    const pageHeight = screen.availHeight;

    console.log(imageContent);

    var base64ImageContent =  "data:image/jpeg;base64,"+imageContent
    _img.setAttribute('src', base64ImageContent);

    _img.width = imageWidth;
    _img.height = imageHeight;



    _img.onload = () => {
        const aspectRatio = _img.width / _img.height;

        canvas.width = _img.width;
        canvas.height = _img.height;

        context.clearRect(0, 0, canvas.width, canvas.height);
        context.drawImage(_img, 0, 0, canvas.width, canvas.height);
    };
}


function plotCordinate(X, Y, Height, Width) {
    let x = 0;
    let y = 0;
    let _rectHeight = 0;
    let _rectWidth = 0;

    x = (X * canvas.width) / 1000;
    y = (Y * canvas.height) / 1000;
    context.setLineDash([]);
    context.strokeStyle = "green";
    context.lineWidth = 2;
    context.fillStyle = "rgba(0, 255, 0, 0.4)";
    context.fillRect(x, y, Width, Height);
}

function clearPlot(X, Y, Height, Width) {
    let x = 0;
    let y = 0;
    let _rectHeight = 0;
    let _rectWidth = 0;

    x = (X * canvas.width) / 1000;
    y = (Y * canvas.height) / 1000;
    height = (Height * canvas.height) / 1000;
    width = (Width * canvas.width) / 1000;
    context.clearRect(x, y, width, height);
    context.drawImage(_img, 0, 0, canvas.width, canvas.height);
}