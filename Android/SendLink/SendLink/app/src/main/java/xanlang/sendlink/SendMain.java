package xanlang.sendlink;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.EditText;

public class SendMain extends AppCompatActivity {

    public static final String EXTRA_MESSAGE = "xanlang.sendlink.MESSAGE";
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_send_main);
    }


    /** Called when the user taps the send button **/
    public void sendLink(View view)
    {
        // check if it's a URL
        // CheckURLString();

        // Send it
        Intent intent = new Intent(this, DisplayMessageActivity.class);
        EditText editText = (EditText) findViewById(R.id.editText);
        String message = editText.getText().toString();
        intent.putExtra(EXTRA_MESSAGE, message);
        startActivity(intent);
    }
}
