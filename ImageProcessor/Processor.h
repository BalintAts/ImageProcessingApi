#pragma once

//using namespace System;

namespace ImageProcessor {
	public ref class Processor {
	public:
		static array<System::Byte>^ Process(array<System::Byte>^ imageBytes, System::String^ encodingType);
	};
}