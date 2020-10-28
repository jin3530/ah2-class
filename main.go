package main

import (
	"ah2-class/webapp"
	"bufio"
	"fmt"
	"io"
	"os"
	"strings"
)

func main() {
	if len(os.Args)<2 {
		fmt.Print(`
使用说明：
将学生账号重置密码后写入到文本文件，每行放一个账号。
运行：ah2-class.exe 账号.txt
`)
		return
	}
	f,err := os.Open(os.Args[1])
	if err!=nil {
		panic(err)
	}
	defer  f.Close()
	rd:=bufio.NewReader(f)
	for  {
		line,_,err:=rd.ReadLine()

		if err!=nil {
			if err == io.EOF {
				break
			}else {
				panic(err)
			}
		}
		{
			res:=strings.Split(string(line)," ")
			var pas string
			if len(res)>1 {
				pas=res[1]
			}else {
				pas="123456"
			}
      		fmt.Print("使用账号 ： "+res[0]+"  结果: ")
			u := webapp.User{UserName: res[0],Password: pas}
			err1 := u.Login()
			if err1 != nil {
				fmt.Println(err1)
				continue
			}
			err1=u.Exam()
			if err1 != nil {
				fmt.Println(err1)
				continue
			}
			fmt.Println(u.String())
		}

	}
}
