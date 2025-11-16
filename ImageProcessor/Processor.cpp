#include "pch.h"
#include "Processor.h"

#include <future>
#include <vector>
#include <algorithm>
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

	static bool openCvInitialized = false;

    static void BlurTile(
        const Mat& src,
        Mat& dst,
        int xStart,
        int yStart,
        int tileW,
        int tileH)
    {
        int kernelRadius = 4;

        int x0 = (std::max)(0, xStart - kernelRadius);
        int x1 = (std::min)(src.cols, xStart + tileW + kernelRadius);

        int y0 = (std::max)(0, yStart - kernelRadius);
        int y1 = (std::min)(src.rows, yStart + tileH + kernelRadius);

        Mat srcTile = src(Range(y0, y1), Range(x0, x1));

        Mat blurredTile;
        GaussianBlur(srcTile, blurredTile, Size(kernelRadius * 2 + 1, kernelRadius * 2 + 1), 2.0);

        int innerX0 = xStart - x0;
        int innerY0 = yStart - y0;

        Rect innerROI(innerX0, innerY0, tileW, tileH);

        blurredTile(innerROI)
            .copyTo(dst(Range(yStart, yStart + tileH),
                Range(xStart, xStart + tileW)));
    }

    void MultiCoreGaussianBlur(const Mat& src, Mat& dst, int tileSize = 256)
    {
        dst.create(src.size(), src.type());

  

        std::vector<std::future<void>> tasks;

        for (int y = 0; y < src.rows; y += tileSize)
        {
            for (int x = 0; x < src.cols; x += tileSize)
            {
                int w = (std::min)(tileSize, src.cols - x);
                int h = (std::min)(tileSize, src.rows - y);

                tasks.push_back(std::async(
                    std::launch::async,
                    [&, x, y, w, h]()
                    {
                        BlurTile(src, dst, x, y, w, h);
                    }
                ));
            }
        }

        for (auto& t : tasks)
            t.get();
    }


	/// <summary>
	/// Applies a gaussian blur for a given image. Returns an empty array if the provided image is invalid.
	/// </summary>
	/// <param name="imageBytes">Image in bytes.</param>
	/// <param name="extension"> The format to into which the image will be encoded by opencv. </param>
	/// <returns>The new image in bytes, encoded into the given format. .</returns>
	array<System::Byte>^ Processor::Process(array<System::Byte>^ imageBytes, System::String^ extension) {
		Mat image = FromBytesToMat(imageBytes);

		if (image.empty())
		{
			return gcnew array<System::Byte>(0);
		}

		Mat blurredImage;
		if (!openCvInitialized) {
			setNumThreads(getNumberOfCPUs());
			openCvInitialized = true;
		}
        MultiCoreGaussianBlur(image, blurredImage);
		return FromMatToBytes(blurredImage, extension);
	}

}
