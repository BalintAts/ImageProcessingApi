#include "pch.h"
#include "Processor.h"

#include <opencv2/opencv.hpp>
#include <msclr/marshal_cppstd.h>


using namespace msclr::interop;
using namespace System::Runtime::InteropServices;
using namespace cv;

namespace ImageProcessor {

	static Mat FromBytesToMat(array<System::Byte>^ imageBytes) {
		std::vector<uchar> buffer(imageBytes->Length);
		Marshal::Copy(imageBytes, 0, System::IntPtr(buffer.data()), imageBytes->Length);
		Mat image = imdecode(buffer, IMREAD_COLOR);
		return image;
	}

	static array<System::Byte>^ FromMatToBytes(Mat image, System::String^ extension) {
		std::vector<uchar> outBuf;
		imencode(marshal_as<std::string>(extension), image, outBuf);
		array<System::Byte>^ result = gcnew array< System::Byte>(outBuf.size());
		Marshal::Copy(System::IntPtr(outBuf.data()), result, 0, outBuf.size());
		return result;
	}

	/// <summary>
	/// Applies a gaussian blur for a given image.
	/// </summary>
	/// <param name="imageBytes">Image in bytes.</param>
	/// <param name="extension"> The format to into which the image will be encoded by opencv. </param>
	/// <returns>The new image in bytes, encoded into the given format.</returns>
	array<System::Byte>^ Processor::Process(array<System::Byte>^ imageBytes, System::String^ extension) {
		Mat image = FromBytesToMat(imageBytes);
		Mat blurredImage;
		setNumThreads(getNumberOfCPUs());
		GaussianBlur(image, blurredImage, Size(9, 9), 2.0);
		return FromMatToBytes(blurredImage, extension);
	}
}
