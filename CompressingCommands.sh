# Windows CMD
"C:\Program Files\7-Zip\7z.exe" a C:\repos\.NET\NATSInternal-Compressed.zip C:\repos\.NET\NATSInternal -xr!*\.vscode -xr!*\.idea -xr!*\.git -xr!*\bin -xr!*\obj -xr!*\*.zip
move C:\repos\.NET\NATSInternal-Compressed.zip C:\repos\.NET\NATSInternal\NATSInternal-Compressed.zip

# Ubuntu/Linux
zip -r ~/repos/NATSInternal-v2/NATSInternal-v2-Compressed.zip ~/repos/NATSInternal-v2 \
    -x "*/.git/*" \
    -x "*/.vscode/*" \
    -x "*/bin/*" \
    -x "*/obj/*" \
    -x "*.zip"