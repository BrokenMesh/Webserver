function submitPost(){
    let error = document.getElementById("error")
    error.innerText = "";

    let name = document.getElementById("name");
    let post = document.getElementById("post");

    if(name.value == "" || post.value == ""){
        error.innerText = "Please fill in all values!";
    } else {
        window.location.href = '/blog/post_was_created.html?' + name.value + "%" + post.value;
    }
}