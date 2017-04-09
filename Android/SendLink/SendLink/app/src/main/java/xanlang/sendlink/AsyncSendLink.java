package xanlang.sendlink;

import android.app.Activity;
import android.content.Context;
import android.os.AsyncTask;
import android.support.v7.app.AppCompatActivity;
import android.widget.TextView;


import java.io.IOException;
import java.io.PrintStream;
import java.net.MalformedURLException;
import java.net.URL;
import java.net.URLConnection;

/**
 * Created by May on 4/8/2017.
 */

public class AsyncSendLink extends AsyncTask<String, Void, Boolean> {
    private Context context;

    public AsyncSendLink(Activity parentActivity)
    {
        context = parentActivity;
    }


    protected Boolean doInBackground(String... params) {

        try {

            String printStreamURL = "&url=" + params[0];

            URL url = new URL("http://www.capsule03.com/sendlink/slink-server.php");
            URLConnection connection = url.openConnection();
            // Activate the output
            connection.setDoOutput(true);

            PrintStream ps = new PrintStream(connection.getOutputStream());

            // send parameters to the site
            ps.print("sendlink=sendlink!anlknvdkj");
            ps.print(printStreamURL);
            ps.print("&username=android");
            ps.close();

            //get input stream in order to send the request
            connection.getInputStream();

            // close the print stream
            ps.close();

        } catch (MalformedURLException e) {
            e.printStackTrace();
            return false;

        } catch (IOException e)
        {
            e.printStackTrace();
            return false;
        }
        return true;
    }

    protected void onPostExecute(Boolean result) {
        TextView textView = (TextView) ((AppCompatActivity) context).findViewById(R.id.textView2);
        textView.setText("Send Success");
    }
}

