#include "pch.h"
#include "Processor.h"


#include <opencv2/opencv.hpp>
#include <msclr/marshal_cppstd.h>


using namespace msclr::interop;
//using namespace System;
//using namespace System::Runtime::InteropServices;
using namespace cv;

namespace ImageProcessor {
	array<System::Byte>^ Processor::Process(array<System::Byte>^ imageBytes) {
		std::vector<uchar> buffer(imageBytes->Length);
		System::Runtime::InteropServices::Marshal::Copy(imageBytes, 0, System::IntPtr(buffer.data()), imageBytes->Length);
		//todo: próbáld ki using system namespace-el

		Mat image = imdecode(buffer, IMREAD_COLOR);
		//if (cvImage.empty())
		//	throw gcnew Exception("");

		Mat blurredImage;
		GaussianBlur(image, blurredImage, Size(9, 9), 2.0);
		std::vector<uchar> outBuf;
		imencode(".jpg", blurredImage, outBuf);

		array<System::Byte>^ result = gcnew array< System::Byte>(outBuf.size());
		System::Runtime::InteropServices::Marshal::Copy(System::IntPtr(outBuf.data()), result, 0, outBuf.size());
		
		return result;
	};
}
