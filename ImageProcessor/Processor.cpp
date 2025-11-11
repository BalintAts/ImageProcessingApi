#include "pch.h"
#include "Processor.h"


#include <opencv2/opencv.hpp>
#include <msclr/marshal_cppstd.h>


using namespace msclr::interop;


namespace ImageProcessor {
	array<System::Byte>^ Processor::Process(array<System::Byte>^ imageBytes) {
		return gcnew array<System::Byte>(0);
	};
}
