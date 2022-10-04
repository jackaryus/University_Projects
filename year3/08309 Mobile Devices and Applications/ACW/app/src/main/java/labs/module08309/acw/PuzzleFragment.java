package labs.module08309.acw;

/**
 * Created by Jack on 17/03/2016.
 */

import android.app.ActionBar;
//import android.support.v4.app.Fragment;
import android.app.Fragment;
import android.content.Context;
import android.content.SharedPreferences;
import android.content.pm.ActivityInfo;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Color;
import android.graphics.drawable.Drawable;
import android.graphics.drawable.Icon;
import android.os.AsyncTask;
import android.os.Bundle;
import android.support.v7.widget.LinearLayoutCompat;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.GridLayout;
import android.widget.ImageButton;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.URL;
import java.util.ArrayList;
import java.util.List;

public class PuzzleFragment extends Fragment {

    private TextView m_PuzzleTextView = null;
    private TextView m_idTextView = null;
    GridLayout m_gridLayout = null;
    Context m_context;
    View view;
    List<String> ImageList;
    List<Bitmap> bitmapList;
    List<Boolean> matchedList;
    List<ImageButton> checkList;
    JSONArray puzzleLayout;
    String pictureSet , selectedPuzzle, SCORE, PUZZLE, MATCHED, BUTTONS;
    int puzzleWidth, puzzleHeight, score, highScore;
    Bitmap cardBack;


    @Override
    public void onAttach(Context context) {
        super.onAttach(context);
        m_context = context;
        this.setRetainInstance(true);
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        Log.i("error", "created view");
        View view = inflater.inflate(R.layout.fragment_puzzle, container, false);
        ImageList = new ArrayList<String>();
        bitmapList = new ArrayList<Bitmap>();
        matchedList = new ArrayList<Boolean>();
        checkList = new ArrayList<ImageButton>();
        SCORE = "score";
        PUZZLE = "puzzle";
        MATCHED = "matched";
        BUTTONS = "buttons";
        cardBack = BitmapFactory.decodeResource(getResources(), R.drawable.cardback);
        m_PuzzleTextView = (TextView)view.findViewById(R.id.puzzleTextView);
        m_idTextView = (TextView)view.findViewById(R.id.idTextView);
        m_gridLayout = (GridLayout)view.findViewById(R.id.puzzleGridView);
        m_PuzzleTextView.setText(getResources().getString(R.string.puzzle));
        String scoreMsg = getResources().getString(R.string.score) + score + getResources().getString(R.string.highscore) + highScore;
        m_idTextView.setText(scoreMsg);
        return view;
    }

    @Override
    public void onPause()
    {
        super.onPause();
        // when paused it will save the selected puzzle name, the current score and the matched list bools
        SharedPreferences.Editor editor = m_context.getSharedPreferences("pref", Context.MODE_PRIVATE).edit();
        editor.putString(PUZZLE, selectedPuzzle);
        editor.putInt(SCORE, score);
        editor.putInt(BUTTONS, matchedList.size());
        for(int i = 0; i < matchedList.size(); i++)
        {
            editor.putBoolean(MATCHED + i, matchedList.get(i));
        }
        editor.apply();
    }

    @Override
    public void onResume()
    {
        super.onResume();
        // will load the variables saved in onPause and construct the matched list and resume the loaded puzzle
        SharedPreferences sharedPreferences = m_context.getSharedPreferences("pref", Context.MODE_PRIVATE);
        selectedPuzzle = sharedPreferences.getString(PUZZLE, "");
        score = sharedPreferences.getInt(SCORE, 0);
        int size = sharedPreferences.getInt(BUTTONS, 0);
        matchedList.clear();
        for(int i = 0; i < size; i++)
        {
            matchedList.add(sharedPreferences.getBoolean(MATCHED + i, false));
        }
        resumePuzzle(selectedPuzzle);
    }

    public void showPuzzle(String puzzle)
    {
        // will get the puzzle name from the text from the list view in setup fragment
        selectedPuzzle = puzzle.replace(".json", "");
        selectedPuzzle = puzzle.split("\r\n")[0];
        matchedList.clear();
        // load the highscore from file if it exists if not set to zero
        try
        {
            FileInputStream reader = m_context.openFileInput(selectedPuzzle+"highscore");
            BufferedReader br = new BufferedReader(new InputStreamReader(reader));
            String readData = "";
            String line;
            while ((line = br.readLine()) != null)
                readData += line;
            highScore = Integer.parseInt(readData);
            Log.i("my error", "highscore retrieved from file");
        }
        catch(Exception k){
            highScore = 0;
            //Log.i("My Exception", k.getMessage());
        }
        score = 0;
        String scoreMsg = getResources().getString(R.string.score) + score + getResources().getString(R.string.highscore) + highScore;
        m_idTextView.setText(scoreMsg);
        //sets text at top of fragment to the puzzle name and initiates the resource collection
        m_PuzzleTextView.setText(selectedPuzzle);
        try {
            new DownloadPuzzle().execute(selectedPuzzle + ".json");

        } catch (Exception e) {
            Log.i("My Exception", e.getMessage());
        }

    }

    public void resumePuzzle(String puzzle)
    {
        // this method is the same as showPuzzle but without the score = 0 and matchedList.clear() as they were both loaded in OnResume
        selectedPuzzle = puzzle.replace(".json", "");
        selectedPuzzle = puzzle.split("\r\n")[0];
        //highscore load
        try
        {
            FileInputStream reader = m_context.openFileInput(selectedPuzzle+"highscore");
            BufferedReader br = new BufferedReader(new InputStreamReader(reader));
            String readData = "";
            String line;
            while ((line = br.readLine()) != null)
                readData += line;
            highScore = Integer.parseInt(readData);
            Log.i("my error", "highscore retrieved from file");
        }
        catch(Exception k){
            highScore = 0;
            //Log.i("My Exception", k.getMessage());
        }
        String scoreMsg = getResources().getString(R.string.score) + score + getResources().getString(R.string.highscore) + highScore;
        m_idTextView.setText(scoreMsg);
        //sets text at top of fragment and initiates the resource collection
        m_PuzzleTextView.setText(selectedPuzzle);
        try {
            new DownloadPuzzle().execute(selectedPuzzle + ".json");

        } catch (Exception e) {
            //Log.i("My Exception", e.getMessage());
        }

    }

    private class DownloadPuzzle extends AsyncTask<String, String, JSONObject> {

        JSONObject jsonObject, m_jsonObject;
        String responseData;
        String id;

        protected JSONObject doInBackground(String... args) {
            try
            {
                FileInputStream reader = m_context.openFileInput(args[0].replace(".json", ".txt"));
                BufferedReader br = new BufferedReader(new InputStreamReader(reader));
                responseData = "";
                String line;
                while ((line = br.readLine()) != null)
                    responseData += line;
                jsonObject = new JSONObject(responseData);
                Log.i("my error", "puzzle retrieved from file");
            }
            catch (Exception e) {
                try {
                    //downloads chosen puzzle from internet
                    String url = "http://www.hull.ac.uk/php/349628/08309/acw/puzzles/" + args[0];
                    InputStream inputStream = (InputStream) new URL(url).getContent();
                    BufferedReader br = new BufferedReader(new InputStreamReader(inputStream));
                    responseData = "";
                    String line;
                    while ((line = br.readLine()) != null)
                        responseData += line;
                    jsonObject = new JSONObject(responseData);
                    Log.i("my error", "puzzle downloaded from internet");
                    //write to file stuff
                    FileOutputStream writerStream = null;
                    try {
                        writerStream = m_context.openFileOutput(args[0].replace(".json", ".txt"), Context.MODE_PRIVATE);
                        writerStream.write(responseData.getBytes(), 0, responseData.length());
                        writerStream.flush();
                        Log.i("my error", "puzzle saved to file");
                    } catch (Exception j) {
                        Log.i("My Error", j.getMessage());
                    } finally {
                        writerStream.close();
                    }
                } catch (Exception k) {
                    //Log.i("My Error", k.getMessage());
                }
            }
            return jsonObject;
        }

        protected void onPostExecute(JSONObject json) {
            if (json != null) {
                try {
                    //extracts the info from the json to be used for setting up the puzzle
                    m_jsonObject = json.getJSONObject("Puzzle");
                    id = m_jsonObject.getString("Id");
                    puzzleWidth = m_jsonObject.getInt("Columns");
                    puzzleHeight = m_jsonObject.getInt("Rows");
                    pictureSet = m_jsonObject.getString("PictureSet");
                    puzzleLayout = m_jsonObject.getJSONArray("Layout");
                } catch (Exception e) {
                    Toast.makeText(m_context, "failed to load puzzle", Toast.LENGTH_SHORT).show();
                }
                PopulatePuzzleGrid(puzzleWidth, puzzleHeight);
            }
        }
    }

    public void PopulatePuzzleGrid(int width, int height)
    {
        //clears grid and sets new width and height
        checkList.clear();
        m_gridLayout.removeAllViewsInLayout();
        m_gridLayout.setColumnCount(width);
        m_gridLayout.setRowCount(height);
        // fills matchedlist if it is empty
        if (matchedList.isEmpty()) {
            for (int i = 0; i < puzzleWidth * puzzleHeight; i++)
                matchedList.add(false);
        }
        //initiates the retrival of images depending on the imageset of the selected puzzle
        try {
            new DownloadImageSet().execute(pictureSet);

        } catch (Exception e) {
            Log.i("My Exception", e.getMessage());
        }
    }

    public void createButtons() {
        int i = 0;
        int p = 0;
        // this method creates the buttons depending on the width and height of the grid and will set the image to the cardback unless the puzzle has been resumed and the tile has been matched
        // then it will be set face up
        for (int y = 0; y < puzzleHeight; y++) {
            for (int x = 0; x < puzzleWidth; x++) {
                ImageButton button = new ImageButton(m_context);
                LinearLayout.LayoutParams params = new LinearLayout.LayoutParams(m_gridLayout.getWidth() / puzzleWidth, m_gridLayout.getHeight() / puzzleHeight);
                button.setLayoutParams(params);
                button.requestLayout();
                try {
                    p = puzzleLayout.getInt(i);
                } catch (Exception e) {
                    Log.i("my error", "puzzle layout error");
                }
                if (matchedList.get(i) == true) {
                    //button.setImageBitmap(cardBack);
                    button.setImageBitmap(bitmapList.get(p - 1));
                } else {
                    button.setImageBitmap(cardBack);
                    //button.setImageBitmap(bitmapList.get(p - 1));
                }
                button.setId(i);
                button.setBackgroundColor(Color.LTGRAY);
                m_gridLayout.addView(button);
                i++;
            }
        }

        int childCount = m_gridLayout.getChildCount();
        // this section sets the on click functionality for the buttons
        for (int j = 0; j < childCount; j++) {
            if (matchedList.get(j) == true) {
                continue;
            }
            final ImageButton button = (ImageButton) m_gridLayout.getChildAt(j);
            button.setOnClickListener(new View.OnClickListener() {
                public void onClick(View view) {
                    // this first section is used for selecting and deselecting a button
                    if (!checkList.contains(button) && !matchedList.get(button.getId())) {
                        checkList.add(button);
                        button.setImageAlpha(200);
                        button.setBackgroundColor(Color.GREEN);
                    } else if (checkList.contains(button)) {
                        checkList.clear();
                        button.setBackgroundColor(Color.LTGRAY);
                        button.setImageAlpha(255);
                    }
                    // if 2 have been selected then they will be fliped and checked to match if they match they will stay face up if not they will be flipped back over
                    if (checkList.size() == 2) {
                        ((MainActivity)getActivity()).LockOri();
                        //checkList.get(0).setImageBitmap(cardBack);
                        //checkList.get(1).setImageBitmap(cardBack);
                        try {
                            checkList.get(0).setImageBitmap(bitmapList.get(puzzleLayout.getInt(checkList.get(0).getId()) - 1));
                            checkList.get(1).setImageBitmap(bitmapList.get(puzzleLayout.getInt(checkList.get(1).getId()) - 1));
                        } catch (JSONException e) {
                            e.printStackTrace();
                        }
                        try {
                            if (puzzleLayout.getInt(checkList.get(0).getId()) == puzzleLayout.getInt(checkList.get(1).getId())) {
                                score += 5;
                                matchedList.set(checkList.get(0).getId(), true);
                                matchedList.set(checkList.get(1).getId(), true);
                                String scoreMsg = getResources().getString(R.string.score) + score + getResources().getString(R.string.highscore) + highScore;
                                m_idTextView.setText(scoreMsg);
                                checkList.get(0).setBackgroundColor(Color.LTGRAY);
                                checkList.get(1).setBackgroundColor(Color.LTGRAY);
                                checkList.clear();
                            } else {
                                score -= 1;
                                String scoreMsg = getResources().getString(R.string.score) + score + getResources().getString(R.string.highscore) + highScore;
                                m_idTextView.setText(scoreMsg);
                                checkList.get(0).setBackgroundColor(Color.LTGRAY);
                                checkList.get(1).setBackgroundColor(Color.LTGRAY);
                                checkList.get(0).setImageAlpha(255);
                                checkList.get(1).setImageAlpha(255);
                                button.postDelayed(new Runnable() {
                                    @Override
                                    public void run() {
                                        // Do something after 5s = 5000ms
                                        checkList.get(0).setImageBitmap(cardBack);
                                        checkList.get(1).setImageBitmap(cardBack);
                                        checkList.clear();
                                    }
                                }, 1000);
                            }
                        } catch (Exception e) {
                            Log.i("My Exception", e.getMessage());
                        }
                        ((MainActivity)getActivity()).UnlockOri();
                    }
                    // if all the tiles have been matched then the score is checked against the highscore if it higher then the score is saved and a win msg is shown and the puzzle is started again
                    if (!matchedList.contains(false)) {
                        if (score > highScore) {
                            Toast.makeText(m_context, R.string.win_highscore, Toast.LENGTH_LONG).show();
                            highScore = score;
                            String scoreMsg = getResources().getString(R.string.score) + score + getResources().getString(R.string.highscore) + highScore;
                            m_idTextView.setText(scoreMsg);
                            //m_idTextView.setText("score:" + score + "highscore:" + highScore);
                            String highscore = Integer.toString(highScore);
                            FileOutputStream writerStream = null;
                            try {
                                writerStream = m_context.openFileOutput(selectedPuzzle + "highscore", Context.MODE_PRIVATE);
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
                            ((MainActivity)getActivity()).UpdatePuzzleList();
                            showPuzzle(selectedPuzzle);
                        } else {
                            Toast.makeText(m_context, R.string.win, Toast.LENGTH_LONG).show();
                            ((MainActivity)getActivity()).UpdatePuzzleList();
                            showPuzzle(selectedPuzzle);
                        }
                    }
                }
            });

        }
    }

    private class DownloadImageSet extends AsyncTask<String, String, JSONObject> {

        JSONObject jsonObject, m_jsonObject;
        String responseData;
        JSONArray m_jsonArray;

        protected JSONObject doInBackground(String... args) {
            // this method gets the image set for the puzzle
            try
            {
                FileInputStream reader = m_context.openFileInput(args[0].replace(".json", ".txt"));
                BufferedReader br = new BufferedReader(new InputStreamReader(reader));
                responseData = "";
                String line;
                while ((line = br.readLine()) != null)
                    responseData += line;
                jsonObject = new JSONObject(responseData);
                Log.i("my error", "image set retrieved from file");
            }
            catch (Exception e) {
                try {
                    String url = "http://www.hull.ac.uk/php/349628/08309/acw/picturesets/" + args[0];
                    InputStream inputStream = (InputStream) new URL(url).getContent();
                    BufferedReader br = new BufferedReader(new InputStreamReader(inputStream));
                    responseData = "";
                    String line;
                    while ((line = br.readLine()) != null)
                        responseData += line;
                    jsonObject = new JSONObject(responseData);
                    Log.i("my error", "image set downloaded from internet");
                    // write to file stuff
                    FileOutputStream writerStream = null;
                    try {
                        writerStream = m_context.openFileOutput(args[0].replace(".json", ".txt"), Context.MODE_PRIVATE);
                        writerStream.write(responseData.getBytes(), 0, responseData.length());
                        writerStream.flush();
                        Log.i("my error", "image set saved to file");
                    } catch (Exception j) {
                        Log.i("My Error", j.getMessage());
                    } finally {
                        writerStream.close();
                    }
                } catch (Exception h) {
                    Log.i("My Error", h.getMessage());
                }
            }
            return jsonObject;
        }

        protected void onPostExecute(JSONObject json) {
            Log.i("my error", "On post execute");
            if (json != null) {
                try {
                    ImageList.clear();
                    m_jsonObject = json.getJSONObject("PictureSet");
                    m_jsonArray = m_jsonObject.getJSONArray("Files");
                    for (int i = 0; i < m_jsonArray.length(); i++)
                    {
                        String p = m_jsonArray.getString(i);
                        ImageList.add(p);
                    }
                } catch (Exception e) {
                    Toast.makeText(m_context, "failed to load imageset.json", Toast.LENGTH_SHORT).show();
                }
                try {
                    new downloadImage().execute(ImageList);
                } catch (Exception e) {
                    Log.i("My Exception", e.getMessage());
                }
            } else {
                Toast.makeText(m_context, "JSON does not exist or network error", Toast.LENGTH_SHORT).show();
            }
        }
    }

    private class downloadImage extends AsyncTask<List<String>, List<String>, List<Bitmap>>
    {
        protected List<Bitmap> doInBackground(List<String>... args)
        {
            //used to load/download the whole imageset for the puzzle to be used for the grid
            bitmapList.clear();
            for (int i = 0; i < ImageList.size(); i++)
            {
                Bitmap bitmap = null;
                try
                {
                    //read image from file if it exists
                    FileInputStream reader = m_context.getApplicationContext().openFileInput(ImageList.get(i));
                    bitmap = BitmapFactory.decodeStream(reader);
                    bitmapList.add(bitmap);
                    //Log.i("My Error", "images loaded from file");
                }
                catch (FileNotFoundException fileNotFound) {
                    try {
                        //if the image was not found download from internet
                        String url = "http://www.hull.ac.uk/php/349628/08309/acw/images/" + ImageList.get(i);
                        bitmap = BitmapFactory.decodeStream((InputStream) new URL(url).getContent());
                        bitmapList.add(bitmap);
                        //then save the image to file
                        FileOutputStream writer = null;
                        try
                        {
                            writer = m_context.getApplicationContext().openFileOutput(ImageList.get(i), Context.MODE_PRIVATE);
                            bitmap.compress(Bitmap.CompressFormat.JPEG, 100, writer);
                        }
                        catch (Exception e)
                        {
                            Log.i("My Error", e.getMessage());
                        }
                        finally
                        {
                            writer.close();
                        }
                    } catch (Exception e) {
                        Log.i("My Error", e.getMessage());
                    }
                }
            }
            return bitmapList;
        }

        protected void onPostExecute(List<Bitmap> images)
        {
            if (images != null)
            {
                //when all images are done calls this method to create buttons in the grid
                createButtons();
            }
            else
            {
                Toast.makeText(m_context, "image does not exist or network error", Toast.LENGTH_SHORT).show();
            }
        }
    }
}
