#filerec

---

> **Feel free to commit!**

Filerec is a Light weight file redundancy checker. It's handy if you don't know if you already have the same files somewhere
on your backup drive. You can compare files by name, size and even if they match content (streams). 

Filerec does not delete the redundant files, this you have to do yourselve

## How to use

```
FILE REDUNDANCY CHECKER:

    Compares two directories if they contain the same files. 
    This can be done on different levels like: 
     - compare filenames
     - compare filesizes 
     - compare filestreams.

    Standard options are: filename: true, filesize: true, filestream: false.
                 
    USAGE:
                   
    filerec [options] source compareto        

    source:     the source directory
    compareto:  compared with this directory       
                 
    OPTIONS:

     -h       Shows this help information,
     -n       DON'T compare filenames
     -l       DON'T copmpare filesizes
     -s       Compare filestreams (This may take some time)
```

## Performance

If you use the stream compare option (`-s`), the comparison could take quiet a while 
(it depends on your file size and the I/O speed of your harddrive).

Also when you compare a huge number of files, this could take quiet some time.
