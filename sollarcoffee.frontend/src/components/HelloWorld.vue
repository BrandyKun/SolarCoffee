<template>
  <div class="hello">
    <h1>{{msg}}</h1>
    <h2> {{myNumber}} </h2>
    <button @click="incrementNumber">Click Me!</button>
  </div>
</template>

<script lang="ts">
import { Options, Vue } from "vue-class-component";

@Options({
  props: {
    msg: String,
  }
})
export default class HelloWorld extends Vue {
  msg!: string;


  number!:number;

  
  myNumber = 0;

  incrementNumber() {
    this.myNumber += 1;
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
h3 {
  margin: 40px 0 0;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  display: inline-block;
  margin: 0 10px;
}
a {
  color: #42b983;
}
</style>
