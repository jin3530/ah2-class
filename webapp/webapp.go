package webapp

import (
	"encoding/json"
	"errors"
	"fmt"
	"io/ioutil"
	"math/rand"
	"net/http"
	"net/http/cookiejar"
	"regexp"
	"strings"
	"time"
)

type User struct {
	UserName string
	name string
	school string
	point int
	jar *cookiejar.Jar
}
type Data struct {
	Point int	`json:"point"`
	UserName string	`json:"userName"`
	SchoolName string	`json:"schoolName"`
}
type Result struct {
	Xdata Data	`json:"data"`
}

type Question struct {
	QuestionId	int 		`json:"questionId"`
	QuestionContent string	`json:"questionContent"`
}
type FormData struct {
	List []Question `json:"list"`
	Time int		`json:"time"`
	Reqtoken string `json:"reqtoken"`
}
var listOK []Question
var listError []Question
func init(){
	listOK=[]Question{
		{QuestionId:2579,QuestionContent: "D"},
		{QuestionId:2643,QuestionContent: "A"},
		{QuestionId:2646,QuestionContent: "A"},
		{QuestionId:2583,QuestionContent: "B"},
		{QuestionId:2586,QuestionContent: "C"},
		{QuestionId:2650,QuestionContent: "B"},
		{QuestionId:2651,QuestionContent: "A"},
		{QuestionId:2653,QuestionContent: "A"},
		{QuestionId:2654,QuestionContent: "A"},
		{QuestionId:2658,QuestionContent: "A"},
		{QuestionId:2595,QuestionContent: "D"},
		{QuestionId:2659,QuestionContent: "A"},
		{QuestionId:2596,QuestionContent: "A"},
		{QuestionId:2598,QuestionContent: "C"},
		{QuestionId:2599,QuestionContent: "D"},
		{QuestionId:2664,QuestionContent: "A"},
		{QuestionId:2603,QuestionContent: "ABCD"},
		{QuestionId:2667,QuestionContent: "B"},
		{QuestionId:2604,QuestionContent: "B"},
		{QuestionId:2669,QuestionContent: "A"},
	}
	listError=[]Question{
		{QuestionId:2579,QuestionContent: "A"},
		{QuestionId:2643,QuestionContent: "B"},
		{QuestionId:2646,QuestionContent: "B"},
		{QuestionId:2583,QuestionContent: "A"},
		{QuestionId:2586,QuestionContent: "A"},
		{QuestionId:2650,QuestionContent: "A"},
		{QuestionId:2651,QuestionContent: "B"},
		{QuestionId:2653,QuestionContent: "B"},
		{QuestionId:2654,QuestionContent: "B"},
		{QuestionId:2658,QuestionContent: "B"},
		{QuestionId:2595,QuestionContent: "C"},
		{QuestionId:2659,QuestionContent: "B"},
		{QuestionId:2596,QuestionContent: "B"},
		{QuestionId:2598,QuestionContent: "D"},
		{QuestionId:2599,QuestionContent: "C"},
		{QuestionId:2664,QuestionContent: "B"},
		{QuestionId:2603,QuestionContent: "CD"},
		{QuestionId:2667,QuestionContent: "A"},
		{QuestionId:2604,QuestionContent: "A"},
		{QuestionId:2669,QuestionContent: "B"},
	}
}
func (u *User) Login() error{
	u.jar,_ = cookiejar.New(nil)
	//headers := {"User-Agent": "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4287.0 Safari/537.36 Edg/88.0.673.0","content-type":"application/json"}
	//client:= &http.Client{Jar: u.jar}
	baseHost := "https://ah.2-class.com/api/anHui/user/anHuiUser_Login?isAnHuiMobile=0"
	client := &http.Client{
		CheckRedirect: func(req *http.Request, via []*http.Request) error {
			return http.ErrUseLastResponse
		},
		Jar: u.jar,
	}
	res, _ := client.Get(baseHost)
	loginurl := res.Header.Get("Location")
	loginurl = strings.Replace(loginurl,"Auth/open-login.html","open-api/v1/users/login",1)
	//提交请求
	reqest, _ := http.NewRequest("POST", loginurl, nil)

	//增加header选项
	reqest.Header.Add("account", u.UserName)
	reqest.Header.Add("password", "123456")

	res, _ = client.Do(reqest)
	res1,_:=ioutil.ReadAll(res.Body)
	defer res.Body.Close()
	m := make(map[string]string)
	err:=json.Unmarshal(res1, &m)
	if err!=nil || m["Message"]!="登录成功"{
		return errors.New("登录失败!")
	}
	client.Get(m["Result"])
	return nil
}


func (u *User) Exam() error{
	client:= &http.Client{Jar: u.jar}
	homeurl := "https://ah.2-class.com/competition"
	ret,_:=client.Get(homeurl)
	res,_:=ioutil.ReadAll(ret.Body)
	defer ret.Body.Close()
	r:=regexp.MustCompile(`reqtoken:"(.+?)"`)
	x:=r.FindStringSubmatch(string(res))
	rand.Seed(time.Now().Unix())
	p:=rand.Intn(6)
	list:=listOK[:(14+p)]
	list = append(list,listError[11+p:]... )
	data:=FormData{List: list,Time: 200+rand.Intn(90),Reqtoken: x[1]}
	fdata,_:=json.Marshal(&data)
	ret1,_:=client.Post("https://ah.2-class.com/api/quiz/commit","application/json",strings.NewReader(string(fdata)))
	ret1,_ = client.Get("https://ah.2-class.com/api/quiz/getQuizCertificate")
	res1,_:=ioutil.ReadAll(ret1.Body)
	defer ret1.Body.Close()
	m:=Result{}
	err:=json.Unmarshal(res1, &m)
	if err!=nil {
		return nil
	}
	u.name = m.Xdata.UserName
	u.point = m.Xdata.Point
	u.school = m.Xdata.SchoolName
	return nil
}

func (u *User) String() string{
	return fmt.Sprintf("%s - %s ,竞赛得分: %d",u.school,u.name,u.point)
}