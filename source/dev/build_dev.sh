#!/bin/bash
echo "Building dev environment..."

#   source path
working_dir=/home/badkraft/repos/razorware/gfxcore/source
dev_dir="$working_dir/dev"
debug="bin/Debug/net9.0"

#   build gfxcore & gfxcore.domain - dependency will cause domain to build
echo "Building gfxcore & gfxcore.domain..."
dotnet build $working_dir/RazorWare.GfxCore/razorware.gfxcore.csproj

#   copy gfxcore.domain to the lib folder
echo "Copying gfxcore & gfxcore.domain to the lib folder..."
yes | cp -f -vi $working_dir/RazorWare.GfxCore/$debug/razorware.gfxcore.dll $dev_dir/lib/
yes | cp -f -vi $working_dir/RazorWare.GfxCore/$debug/razorware.gfxcore.domain.dll $dev_dir/lib/

#   build gfxcore.application
echo "Building gfxcore.application..."
dotnet build $dev_dir/GfxCore.Application/gfxcore.application.csproj
#   copy gfxcore to the gfxcore.application debug directory
echo "Copying gfxcore.dll to the gfxcore.application $debug directory..."
yes | cp -f -vi $dev_dir/lib/razorware.gfxcore.dll $dev_dir/GfxCore.Application/$debug/

