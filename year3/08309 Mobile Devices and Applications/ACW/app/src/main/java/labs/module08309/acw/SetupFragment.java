package labs.module08309.acw;

import android.content.Context;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.os.AsyncTask;
import android.os.Bundle;
import android.os.Debug;
import android.app.Fragment;
//import android.support.v4.app.Fragment;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import org.json.JSONArray;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.DataOutput;
import java.io.DataOutputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.URL;
import java.util.ArrayList;
import java.util.List;

/**
 * Created by Jack on 17/03/2016.
 */
public class SetupFragment extends Fragment {
    ArrayAdapter<String> mAdapter;
    String responseData;
    List<String> listItems;
    Context m_context;
    ListView listView;
    View view;

    @Override
    public void onAttach(Context context)
    {
        super.onAttach(context);
        m_context = context;
        this.setRetainInstance(true);
        try {
            m_Listener = (ListSelectionListener) context;
        }
        catch(ClassCastException e){
            throw new ClassCastException(context.toString()+ " must implement ListSelectionListener");
        }
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_setup, container, false);
        TextView textView = (TextView)view.findViewById(R.id.textView);
        textView.setText(getResources().getString(R.string.puzzle_select_msg));
        ListView listView = (ListView)view.findViewById(R.id.listView);
        listItems = new ArrayList<String>();
        // starts the download of the index
        try
        {
            new DownloadIndex().execute("index.json");

        } catch (Exception e) {
            Log.i("My Exception", e.getMessage());
        }
        // setting data and adapter for list view
        ArrayAdapter<String> arrayAdapter = new ArrayAdapter<String>(m_context, android.R.layout.simple_list_item_1, android.R.id.text1, listItems);
        listView.setAdapter(arrayAdapter);
        // on item click will update the list and call method in main activity
        listView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                UpdateList(position);
                m_Listener.onListSelection(listItems.get(position), position);
            }
        });
        return view;
    }

    private ListSelectionListener m_Listener = null;

    public interface ListSelectionListener{
        public void onListSelection(String puzzle, int pos);

    }

    public void UpdateList(int pos) {
        int highScore;
        // checks for highscore if one doesnt exist its set to zero
        try {
            FileInputStream reader = m_context.openFileInput(listItems.get(pos).split("\r\n")[0] + "highscore");
            BufferedReader br = new BufferedReader(new InputStreamReader(reader));
            String readData = "";
            String line;
            while ((line = br.readLine()) != null)
                readData += line;
            highScore = Integer.parseInt(readData);
            Log.i("my error", "highscore retrieved from file");
        } catch (Exception k) {
            highScore = 0;
            String highscore = Integer.toString(highScore);
            FileOutputStream writerStream = null;
            try {
                writerStream = m_context.openFileOutput(listItems.get(pos).split("\r\n")[0] + "highscore", Context.MODE_PRIVATE);
                writerStream.write(highscore.getBytes(), 0, highscore.length());
                writerStream.flush();
                Log.i("my error", "highscore set saved to file");
            } catch (Exception j) {
                Log.i("My Error", j.getMessage());
            } finally {
                try {
                    writerStream.close();
                } catch (Exception f) {
                    Log.i("My Exception", f.getMessage());
                }
            }
            //Log.i("My Exception", k.getMessage());
        }
        // if its already been updated omce then it will only update the values
        if (!listItems.get(pos).contains("Highscore"))
        {
            listItems.set(pos, listItems.get(pos) + "\r\nHighscore:" + highScore);
            mAdapter = new ArrayAdapter<String>(m_context, android.R.layout.simple_list_item_1, listItems);
            listView.setAdapter(mAdapter);
        }
        // if it hasnt it will add the extra characters and highscore
        else
        {
            listItems.set(pos, listItems.get(pos).split("\r\n")[0] + "\r\nHighscore:" + highScore);
            mAdapter = new ArrayAdapter<String>(m_context, android.R.layout.simple_list_item_1, listItems);
            listView.setAdapter(mAdapter);
        }
    }

    private class DownloadIndex extends AsyncTask<String, String, JSONObject> {

        JSONObject jsonObject;
        // will download index before load from file in case new puzzles are added or removed
        protected JSONObject doInBackground(String... args) {
                try {
                    String url = "http://www.hull.ac.uk/php/349628/08309/acw/index.json";
                    InputStream inputStream = (InputStream) new URL(url).getContent();
                    BufferedReader br = new BufferedReader(new InputStreamReader(inputStream));
                    responseData = "";
                    String line;
                    while ((line = br.readLine()) != null)
                        responseData += line;
                    jsonObject = new JSONObject(responseData);
                    // write to file stuff
                    Log.i("my error", "index downloaded from internet");
                    FileOutputStream writerStream = null;
                    try {
                        writerStream = m_context.openFileOutput("index.txt", Context.MODE_PRIVATE);
                        writerStream.write(responseData.getBytes(), 0, responseData.length());
                        writerStream.flush();
                        Log.i("my error", "index saved to file");
                    } catch (Exception j) {
                        Log.i("My Error", j.getMessage());
                    } finally {
                        writerStream.close();
                    }
                }
                catch (Exception k) {
                    Log.i("My Error", k.getMessage());
                    try {
                        FileInputStream reader = m_context.openFileInput("index.txt");
                        BufferedReader br = new BufferedReader(new InputStreamReader(reader));
                        responseData = "";
                        String line;
                        while ((line = br.readLine()) != null)
                            responseData += line;
                        jsonObject = new JSONObject(responseData);
                        Log.i("my error", "index retrieved from file");
                    } catch (Exception l) {
                        Log.i("My Error", l.getMessage());
                    }
                }

            return jsonObject;
        }

        protected void onPostExecute(JSONObject json) {
            JSONArray JsonArray;
            if (json != null) {
                try {
                    JsonArray = json.getJSONArray("PuzzleIndex");
                    for (int i = 0; i < JsonArray.length(); i++)
                    {
                        String o = JsonArray.getString(i);
                        int highScore;
                        // if a highscore exists it will show it before a puzzle is selected and the list updated otherwise it will just add the puzzle name
                        try
                        {
                            FileInputStream reader = m_context.openFileInput(o.replace(".json", "")+"highscore");
                            BufferedReader br = new BufferedReader(new InputStreamReader(reader));
                            String readData = "";
                            String line;
                            while ((line = br.readLine()) != null)
                                readData += line;
                            highScore = Integer.parseInt(readData);
                            listItems.add(o.replace(".json", "") + "\r\nHighscore:" + highScore);
                            Log.i("my error", "highscore retrieved from file");
                        }
                        catch(Exception k){
                            listItems.add(o.replace(".json", ""));
                            //Log.i("My Exception", k.getMessage());
                        }
                    }
                } catch (Exception e) {
                    listItems.add("failed to load json to list view :(");
                }
                listView = (ListView) view.findViewById(R.id.listView);
                mAdapter = new ArrayAdapter<String>(m_context, android.R.layout.simple_list_item_1, listItems);
                listView.setAdapter(mAdapter);
                listView.setTextFilterEnabled(true);
            } else {
                Toast.makeText(m_context, "JSON does not exist or network error", Toast.LENGTH_SHORT).show();
            }
        }
    }

}
