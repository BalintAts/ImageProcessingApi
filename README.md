## ImageProcessingApi

This application exposes an endpoint where you can upload an image. After uploading, a blurred version of the image will be returned in the response.

### How to use it in developer mode

The application uses the OpenCV library for image processing, which you need to install first.
When you run the application, go to https://localhost:7122/swagger/index.html (it should open automatically).
You will see an HTTP POST endpoint with two parameters: the image you want to process, and the encodingType, which specifies the format of the output image file.
After you press Execute, the processed (blurred) image will appear below.


