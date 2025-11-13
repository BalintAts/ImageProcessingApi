#pragma once

namespace ImageProcessor {
	public ref class Processor {
	public:
		static array<System::Byte>^ Process(array<System::Byte>^ imageBytes, System::String^ encodingType);
	};
}