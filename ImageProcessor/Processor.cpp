#include "pch.h"
#include "Processor.h"


#include <opencv2/opencv.hpp>
#include <msclr/marshal_cppstd.h>


using namespace msclr::interop;
//using namespace System;
//using namespace System::Runtime::InteropServices;
using namespace cv;

/// <summary>
/// process, encode
/// </summary>
namespace ImageProcessor {


	static Mat FromBytesToMat(array<System::Byte>^ imageBytes) {
		std::vector<uchar> buffer(imageBytes->Length);
		System::Runtime::InteropServices::Marshal::Copy(imageBytes, 0, System::IntPtr(buffer.data()), imageBytes->Length);
		Mat image = imdecode(buffer, IMREAD_COLOR);
		if (image.empty())
			throw gcnew System::Exception("");  //todo: custom exception
		return image;
	}

	static array<System::Byte>^ FromMatToBytes(Mat image, System::String^ extension) {
		std::vector<uchar> outBuf;
		imencode(marshal_as<std::string>(extension), image, outBuf);
		array<System::Byte>^ result = gcnew array< System::Byte>(outBuf.size());
		System::Runtime::InteropServices::Marshal::Copy(System::IntPtr(outBuf.data()), result, 0, outBuf.size());
		return result;
	}
	/// <summary>
	/// 
	/// </summary>
	/// <param name="imageBytes"></param>
	/// <param name="extension"> The format to into which the image will be encoded by opencv. </param>
	/// <returns></returns>
	array<System::Byte>^ Processor::Process(array<System::Byte>^ imageBytes, System::String^ extension) {
		//std::vector<uchar> buffer(imageBytes->Length);
		//System::Runtime::InteropServices::Marshal::Copy(imageBytes, 0, System::IntPtr(buffer.data()), imageBytes->Length);
		////todo: próbáld ki using system namespace-el

		//Mat image = imdecode(buffer, IMREAD_COLOR);
		//if (cvImage.empty())
		//	throw gcnew Exception("");
		Mat image = FromBytesToMat(imageBytes);
		Mat blurredImage;
		cv::setNumThreads(cv::getNumberOfCPUs());
		GaussianBlur(image, blurredImage, Size(9, 9), 2.0);
		//std::vector<uchar> outBuf;
		//imencode(marshal_as<std::string>(extension), blurredImage, outBuf);

		//array<System::Byte>^ result = gcnew array< System::Byte>(outBuf.size());
		//System::Runtime::InteropServices::Marshal::Copy(System::IntPtr(outBuf.data()), result, 0, outBuf.size());

		return FromMatToBytes(blurredImage, extension);
	}




}
